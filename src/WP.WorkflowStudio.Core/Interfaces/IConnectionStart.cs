namespace WP.WorkflowStudio.Core.Interfaces;

public interface IConnectionStart
{
    int GetId();

    void AddConnection(IConnectionEnd connection);

    IEnumerable<IConnectionEnd> GetConnections();
}