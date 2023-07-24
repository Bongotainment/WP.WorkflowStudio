using System.Runtime.Versioning;
using WP.WorkflowStudio.Core.Interfaces;

namespace WP.WorkflowStudio.Core.Models;

public class Workflow : IConnectionStart
{
    private readonly List<IConnectionEnd> _events = new();
    private readonly List<String> _actionTexts = new List<string>();
    private readonly List<String> _conditionTexts= new List<string>(); 

    public int WorkflowId { get; set; }

    public string? Name { get; set; }

    public bool Activated { get; set; }

    public int Position { get; set; }

    public bool ExecuteDelayed { get; set; }

    public bool WasExecutedInPast { get; set; }

    public bool HasCustomCondition { get; set; }

    public bool IsCustomWorkflow { get; set; }

    public void AddSearchableActionText(string text)
    {
        this._actionTexts.Add(text);
    }

    public String[] GetSearchableActionTexts()
    {
        return _actionTexts.ToArray();
    }

    public void AddSearchableConditionsText(string text)
    {
         this._conditionTexts.Add(text);
    }

    public String[] GetSearchableConditionsTexts()
    {
        return this._conditionTexts.ToArray();
    }

    public int GetId()
    {
        return WorkflowId;
    }

    public void AddConnection(IConnectionEnd connection)
    {
        _events.Add(connection);
    }

    public IEnumerable<IConnectionEnd> GetConnections()
    {
        return _events;
    }
}