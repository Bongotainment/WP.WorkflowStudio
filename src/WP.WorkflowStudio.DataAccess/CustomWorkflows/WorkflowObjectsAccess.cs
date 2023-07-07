using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.CustomWorkflows;

public class WorkflowObjectsAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowObjectsAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public List<TWorkflowObjects> Get()
    {
        var query = "SELECT * FROM CustomWorkflows.tWorkflowObjects";
        var workflowObjectsList = _connection.Query<TWorkflowObjects>(query);
        return workflowObjectsList;
    }
}