using System.Drawing;
using WP.WorkflowStudio.Core.Interfaces;

namespace WP.WorkflowStudio.Core.Models;

public class WorkflowEvent : IConnectionEnd
{
    private List<Workflow> _children { get; } = new();

    public int EventId { get; set; }
    public int ObjectId { get; set; }
    public string? Name { get; set; }

    public Color? Color { get; set; }


    public int GetId()
    {
        return EventId;
    }

    public int GetObjectId()
    {
        return ObjectId;
    }

    public IEnumerable<IConnectionStart> GetConnections()
    {
        return _children;
    }

    public bool WasExecutedInPast()
    {
        return _children.Any(x => x.WasExecutedInPast);
    }

    public void AddChild(Workflow workflow)
    {
        _children.Add(workflow);
    }

    public IEnumerable<Workflow> GetWorkflows()
    {
        return _children.ToList(); // Shallow copy
    }
}