namespace WP.WorkflowStudio.Visuals.Canvas;

public class ElementClickedEventArgs
{
    public ElementClickedEventArgs(string eventName, string avarageRuntime, string flowName, bool isCustomWorkflow
        , bool wasExecutedInPast, bool hasCustomCondition, bool executeDelayed, bool activated)
    {
        WasExecutedInPast = wasExecutedInPast;
        EventName = eventName;
        AvarageRuntime = avarageRuntime;
        FlowName = flowName;
        IsCustomWorkflow = isCustomWorkflow;
        HasCustomCondition = hasCustomCondition;
        ExecuteDelayed = executeDelayed;
        Activated = activated;
    }

    public string EventName { get; }
    public string AvarageRuntime { get; }
    public string FlowName { get; }
    public bool IsCustomWorkflow { get; }
    public bool WasExecutedInPast { get; }
    public bool HasCustomCondition { get; }
    public bool ExecuteDelayed { get; }
    public bool Activated { get; }
}