namespace WP.WorkflowStudio.Core.Interfaces;

public interface IConnectionEnd
{
    int GetId();

    int GetObjectId();

    IEnumerable<IConnectionStart> GetConnections();
}