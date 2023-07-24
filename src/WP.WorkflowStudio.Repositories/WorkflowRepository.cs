using System.Runtime.Serialization;
using System.Text;
using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.Core.Models;
using WP.WorkflowStudio.DataAccess.DBO;
using WP.WorkflowStudio.DataAccess.Workflows;
using WP.WorkflowStudio.Repositories.Helpers;
using WP.WorkflowStudio.Repositories.Objects;

namespace WP.WorkflowStudio.Repositories;

//This class is ugly as f&ck.
//Todo Refactor the linq statements
public class WorkflowRepository
{
    private readonly EventNameTransformer _eventNameTransformer;
    private readonly Dictionary<int, DateTime> _logCache;
    private readonly RMStatusAccess _rmStatusAccess;
    private readonly RMStatusSpracheAccess _rmStatusSpracheAccess;
    private readonly RMStatusWorkflowAccess _rmStatusWorkflowAccess;
    private readonly WorkflowAccess _workflowAccess;
    private readonly WorkflowAktionAccess _workflowAktionAccess;
    private readonly WorkflowBedingungAccess _workflowBedingungAccess;
    private readonly WorkflowEigenschaftAccess _workflowEigenschaftAccess;
    private readonly WorkflowEventAccess _workflowEventAccess;
    private readonly WorkflowLogAccess _workflowLogAccess;

    private IEnumerable<WorkflowEventCombination>? _cache;
    private IDictionary<int, List<jtlAktionCustomWorkflow>>? _customWorkflows;

    private IDictionary<int, List<jtlAktionWorkflowStarten>>? _workflowEventConnections;

    public WorkflowRepository(WorkflowAccess workflowAccess,
        WorkflowAktionAccess workflowAktionAccess,
        WorkflowBedingungAccess workflowBedingungAccess,
        WorkflowEigenschaftAccess workflowEigenschaftAccess,
        WorkflowEventAccess workflowEventAccess,
        RMStatusAccess rmStatusAccess,
        RMStatusWorkflowAccess rmStatusWorkflowAccess,
        RMStatusSpracheAccess rmStatusSpracheAccess,
        WorkflowLogAccess workflowLogAccess)
    {
        _workflowAccess = workflowAccess;
        _workflowAktionAccess = workflowAktionAccess;
        _workflowBedingungAccess = workflowBedingungAccess;
        _workflowEigenschaftAccess = workflowEigenschaftAccess;
        _workflowEventAccess = workflowEventAccess;
        _rmStatusAccess = rmStatusAccess;
        _rmStatusWorkflowAccess = rmStatusWorkflowAccess;
        _rmStatusSpracheAccess = rmStatusSpracheAccess;
        _workflowLogAccess = workflowLogAccess;
        _eventNameTransformer = new EventNameTransformer();
        _logCache = new Dictionary<int, DateTime>();
    }

    public IEnumerable<WorkflowEvent> GetWorkflows(int nObject)
    {
        return GetWorkflows(new[] { nObject });
    }

    private IEnumerable<ReturnWorkflowEreignisName> GetReturnWorkflows()
    {
        var flows = _workflowAccess.Get<TWorkflow>();
        var rmstatus = _rmStatusAccess.Get<TRmStatus>();
        var rmstatusworkflows = _rmStatusWorkflowAccess.Get<TRmStatusWorkflow>();
        var rmstatussprache = _rmStatusSpracheAccess.Get<TRmStatusSprache>();

        var result = from trms in rmstatus
            join trmss in rmstatussprache
                on trms.KRmStatus equals trmss.KRmStatus
            join trmsw in rmstatusworkflows
                on trms.KRmStatus equals trmsw.KRmStatus
            join trmssParent in rmstatussprache
                on trms.KRmStatusParent equals trmssParent.KRmStatus
                into leftOuter
            from trmssParent in leftOuter.DefaultIfEmpty()
            join tw in flows
                on trmsw.KWorkflow equals tw.KWorkflow
            where trmssParent?.KSprache == null || trmssParent.KSprache == 1
            select new
            {
                Art = tw.NObjekt == 15 ? "Retoure" : tw.NObjekt == 14 ? "Retourenposition" : "",
                Typ = trmsw.NTyp == 1 ? "Retourenstatus erreicht" :
                    trmsw.NTyp == 2 ? "Retourenstatus verlassen" :
                    trmsw.NTyp == 3 ? "Positionsstatus erreicht" :
                    trmsw.NTyp == 4 ? "Positionsstatus verlassen" : "",
                StatusName = trmss.CName,
                WorkflowName = tw.CName,
                trms.KRmStatus,
                trms.KRmStatusParent,
                trms.NAktiv,
                tw.KWorkflow
            };

        var results = result.Select(x => new ReturnWorkflowEreignisName
            { Ereignis = x.StatusName + " " + x.Typ, KWorkflow = x.KWorkflow });
        return results.ToList();
    }

    private Dictionary<int, List<String>> _workflowActionsCache = new Dictionary<int, List<String>>();
    private Dictionary<int, List<String>> _workflowConditionsCache = new Dictionary<int, List<String>>();

    public IEnumerable<WorkflowEvent> GetWorkflows(int[] nObject)
    {
        var results = new List<WorkflowEvent>();
        var returnWorkflowEreignisNames = GetReturnWorkflows();
        var conditions = _workflowBedingungAccess.Get<TWorkflowBedingung>();
        
        if (_cache == null || !_cache.Any())
        {
            _cache = new List<WorkflowEventCombination>();
            var flows = _workflowAccess.Get<TWorkflow>();
            var actions = _workflowAktionAccess.Get<TWorkflowAktion>();
            var properties = _workflowEigenschaftAccess.Get<TWorkflowEigenschaft>();
            var logs = _workflowLogAccess.GetMiniGrouped<TWorkflowLog>();
            var events = _workflowEventAccess.Get<TWorkflowEvent>();
            _workflowEventConnections = GetJTLAktionWorkflows(actions);
            _customWorkflows = GetjtlAktionCustomWorkflows(actions);
            var query =
                from flow in flows
                join evt in events on flow.NEvent equals evt.NEvent into leftOuter
                from lEvent in leftOuter.DefaultIfEmpty()
                select new WorkflowEventCombination(flow, lEvent);
            _cache = query.ToList();

            _workflowActionsCache = TransformActions(actions);
            _workflowConditionsCache = TransformConditions(conditions);
            var logQuery = from log in logs
                group log by log.KWorkflow
                into g
                select new
                {
                    kWorkflow = g, dDatum = g.Max(log => log.DDatum)
                };
            foreach (var queryObj in logQuery)
            {
                if (queryObj == null || queryObj.kWorkflow == null) continue;
                _logCache[queryObj.kWorkflow.Key] = queryObj.dDatum;
            }
        }

        List<WorkflowEventCombination> currentFlows;
        if (nObject.Length > 0)
            currentFlows = _cache.Where(x => nObject.Contains(x.flow.NObjekt)).ToList();
        else
            currentFlows = _cache.ToList();

        var customConditions = FilterCustomConditionsFromConditions(conditions);
        foreach (var flowEventCombination in currentFlows)
        {
            var workflow = new Workflow();
            workflow.Name = flowEventCombination.flow.CName;
            workflow.WorkflowId = flowEventCombination.flow.KWorkflow;
            workflow.Activated = flowEventCombination.flow.NTyp <= 0;
            workflow.Position = flowEventCombination.flow.NPos;
            workflow.ExecuteDelayed = flowEventCombination.flow.NSchedulerOptions > 2;
            workflow.HasCustomCondition = customConditions.Any(x => x.KWorkflow == workflow.WorkflowId);
            workflow.WasExecutedInPast = _logCache.Any(x => x.Key == workflow.WorkflowId);

            FillWorkflowActionTexts(workflow);
            FillWorkflowConditionTexts(workflow);
            
            if (_customWorkflows != null)
                workflow.IsCustomWorkflow = _customWorkflows.Any(x => x.Key == workflow.WorkflowId);

            var element = results.FirstOrDefault(evnt =>
                evnt.EventId == flowEventCombination.flow.NEvent && evnt.ObjectId == flowEventCombination.flow.NObjekt);
            if (element == null)
            {
                var name = returnWorkflowEreignisNames.FirstOrDefault(x => x.KWorkflow == workflow.WorkflowId);
                if (flowEventCombination.lEvent == null)
                    element = GetStandardEvents(flowEventCombination.flow, name);
                else
                    element = GetNonStandardEvents(flowEventCombination.lEvent, name);

                element.AddChild(workflow);
                results.Add(element);
            }
            else
            {
                element.AddChild(workflow);
            }
        }

        if (_workflowEventConnections != null)
            AddWorkflowConnections(results.ToList<IConnectionEnd>(), _workflowEventConnections);
        return results;
    }

    private void FillWorkflowConditionTexts(Workflow workflow)
    {
        foreach (var (workflowId, conditionTexts) in _workflowConditionsCache)
        {
            if (workflowId == workflow.WorkflowId)
            {
                foreach (string text in conditionTexts)
                {
                    workflow.AddSearchableConditionsText(text);
                }
            }
        }
    }

    private void FillWorkflowActionTexts(Workflow workflow)
    {
        foreach (var (workflowId, actionTexts) in _workflowActionsCache)
        {
            if (workflowId == workflow.WorkflowId)
            {
                foreach (string text in actionTexts)
                {
                    workflow.AddSearchableActionText(text);
                }
            }
        }
    }

    private Dictionary<int,List<string>> TransformActions(IEnumerable<TWorkflowAktion> actions)
    {
        var result = actions.GroupBy(x => x.KWorkflow);
        Dictionary<int, List<string>> returnValue = result.ToDictionary(obj => obj.Key, obj => obj.Select(x => x.XXmlObjekt).ToList());
        return returnValue;
    }
    
    private Dictionary<int,List<string>> TransformConditions(IEnumerable<TWorkflowBedingung> conditions)
    {
        var grouped = conditions.GroupBy(x => x.KWorkflow);
        Dictionary<int, List<string>> returnValue = grouped.ToDictionary(obj => obj.Key, obj => obj.Select(x => x.CEigenschaft).ToList());
        Dictionary<int, List<string>?> returnValue2 = grouped.ToDictionary(obj => obj.Key, obj => obj.Select(x => x.CVergleichswert).ToList());

        foreach (var kvp in returnValue)
        {
            List<string>? foundValues;
            if (returnValue2.TryGetValue(kvp.Key, out foundValues))
            {
                if (foundValues != null) 
                    returnValue[kvp.Key].AddRange(foundValues);
            }
        }
        return returnValue;
    }
    

    private static List<TWorkflowBedingung> FilterCustomConditionsFromConditions(
        IEnumerable<TWorkflowBedingung> conditions)
    {
        List<TWorkflowBedingung> customConditions;
        var x = conditions.Where(x => x.CEigenschaft?.Contains("propertyName=\"_workflowEigenschaft\"") == true);
        if (x != null)
            customConditions = x.ToList();
        else
            customConditions = new List<TWorkflowBedingung>();

        return customConditions;
    }

    public IEnumerable<WorkflowEvent> GetArticleWorkflows()
    {
        return GetWorkflows(new[] { 1, 2, 11 });
    }

    public IEnumerable<WorkflowEvent> GetProcurementWorkflows()
    {
        return GetWorkflows(new[] { 3, 4 });
    }

    public IEnumerable<WorkflowEvent> GetCustomersWorkflows()
    {
        return GetWorkflows(5);
    }

    public IEnumerable<WorkflowEvent> GetOrderWorkflows()
    {
        return GetWorkflows(6);
    }

    public IEnumerable<WorkflowEvent> GetDeliveryWorkflows()
    {
        return GetWorkflows(new[] { 7, 16 });
    }

    public IEnumerable<WorkflowEvent> GetInvoiceWorkflows()
    {
        return GetWorkflows(new[] { 8, 9 });
    }

    public IEnumerable<WorkflowEvent> GetPicklistWorkflows()
    {
        return GetWorkflows(new[] { 10, 18 });
    }

    public IEnumerable<WorkflowEvent> GetOfferWorkflows()
    {
        return GetWorkflows(12);
    }

    public IEnumerable<WorkflowEvent> GetServicedeskWorkflows()
    {
        return GetWorkflows(20);
    }

    public IEnumerable<WorkflowEvent> GetReturnsWorkflows()
    {
        return GetWorkflows(new[] { 14, 15 });
    }

    private static void AddWorkflowConnections(List<IConnectionEnd> results,
        IDictionary<int, List<jtlAktionWorkflowStarten>> workflowEventConnections)
    {
        foreach (var evt in results)
        foreach (var connectionStart in evt.GetConnections())
            if (workflowEventConnections.TryGetValue(connectionStart.GetId(), out var workflowActions))
                foreach (var workflowAction in workflowActions)
                {
                    var _foundEvents = results.FirstOrDefault(x => x.GetId() == workflowAction.WorkflowEvent);
                    if (_foundEvents != null) connectionStart.AddConnection(_foundEvents);
                }
    }

    private static IDictionary<int, List<jtlAktionWorkflowStarten>> GetJTLAktionWorkflows(
        IEnumerable<TWorkflowAktion> actions)
    {
        var results = new Dictionary<int, List<jtlAktionWorkflowStarten>>();
        foreach (var action in actions)
            try
            {
                var xml = action.XXmlObjekt;
                xml = xml.Replace("xmlns=\"jtlCore.Workflows.Aktionen\"", "xmlns=\"WP.WorkflowStudio.Repositories\"");
                xml = xml.Replace("xmlns:a=\"http://schemas.datacontract.org/2004/07/jtlCore.Workflows.Aktionen\"",
                    "xmlns:a=\"http://schemas.datacontract.org/2004/07/WP.WorkflowStudio.Repositories\"");

                var serializer = new DataContractSerializer(typeof(jtlAktion));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var result = (jtlAktion?)serializer.ReadObject(stream);
                    if (result?.GetType() == typeof(jtlAktionWorkflowStarten))
                    {
#pragma warning disable CS8604
                        if (results.TryGetValue(action.KWorkflow, out var worfklowStarten))
                        {
                            worfklowStarten.Add(result as jtlAktionWorkflowStarten);
                            results[action.KWorkflow] = worfklowStarten;
                        }
                        else
                        {
                            worfklowStarten = new List<jtlAktionWorkflowStarten> { result as jtlAktionWorkflowStarten };
                            results.Add(action.KWorkflow, worfklowStarten);
                        }

#pragma warning restore CS8604
                    }
                }
            }
            catch (Exception)
            {
                //Ignore
            }

        return results;
    }

    private static IDictionary<int, List<jtlAktionCustomWorkflow>> GetjtlAktionCustomWorkflows(
        IEnumerable<TWorkflowAktion> actions)
    {
        var results = new Dictionary<int, List<jtlAktionCustomWorkflow>>();
        foreach (var act in actions)
        {
            if (act == null || act.XXmlObjekt == null) continue;

            try
            {
                var xml = act.XXmlObjekt;
                xml = xml.Replace("xmlns=\"jtlCore.Workflows.Aktionen\"", "xmlns=\"WP.WorkflowStudio.Repositories\"");
                xml = xml.Replace("xmlns:a=\"http://schemas.datacontract.org/2004/07/jtlCore.Workflows.Aktionen\"",
                    "xmlns:a=\"http://schemas.datacontract.org/2004/07/WP.WorkflowStudio.Repositories\"");

                var serializer = new DataContractSerializer(typeof(jtlAktion));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var result = (jtlAktion?)serializer.ReadObject(stream);

                    if (result != null && result.GetType() == typeof(jtlAktionCustomWorkflow))
                    {
                        var customWorkflow = result as jtlAktionCustomWorkflow;
                        if (customWorkflow == null) continue;

                        if (results.TryGetValue(act.KWorkflow, out var customWorkflows))
                        {
                            customWorkflows.Add(customWorkflow);
                            results[act.KWorkflow] = customWorkflows;
                        }
                        else
                        {
                            customWorkflows = new List<jtlAktionCustomWorkflow> { customWorkflow };
                            results.Add(act.KWorkflow, customWorkflows);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Ignore
            }
        }

        return results;
    }

    private static WorkflowEvent GetNonStandardEvents(TWorkflowEvent workflowEvent,
        ReturnWorkflowEreignisName? returnWorkflowEreignisName)
    {
        var element = new WorkflowEvent();
        if (string.IsNullOrEmpty(returnWorkflowEreignisName?.Ereignis))
        {
            if (!string.IsNullOrEmpty(workflowEvent.CDisplayName))
                element.Name = workflowEvent.CDisplayName;
            else
                element.Name = "???";
        }
        else
        {
            element.Name = returnWorkflowEreignisName?.Ereignis;
        }


        element.EventId = workflowEvent.NEvent;
        element.ObjectId = workflowEvent.NObjekt;
        return element;
    }

    private WorkflowEvent GetStandardEvents(TWorkflow workflow, ReturnWorkflowEreignisName? returnWorkflowEreignisName)
    {
        var eventName = string.Empty;
        if (string.IsNullOrEmpty(returnWorkflowEreignisName?.Ereignis))
            eventName = _eventNameTransformer.GetEventName(workflow.NObjekt, workflow.NEvent);
        else
            eventName = _eventNameTransformer.GetEventName(workflow.NObjekt, workflow.NEvent) + " " +
                        returnWorkflowEreignisName?.Ereignis;
        var element = new WorkflowEvent
        {
            Name = eventName,
            EventId = workflow.NEvent,
            ObjectId = workflow.NObjekt
        };
        return element;
    }
}