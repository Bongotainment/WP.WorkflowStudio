namespace WP.WorkflowStudio.Core.Searching;

public class SearchValues
{
    public SearchValues(string name, string conditionText, string actionText)
    {
        Name = name;
        ConditionText = conditionText;
        ActionText = actionText;
    }

    public String Name { get; private set; }
    public String ConditionText { get; private set; }
    public String ActionText { get; set; }
}