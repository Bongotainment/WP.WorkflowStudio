using WP.WorkflowStudio.DataAccess.CustomWorkflows.Records;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.CustomWorkflows;

public class CustomActionAccess
{
    private readonly SQLServerConnection _connection;

    public CustomActionAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public List<VCustomAction> Get()
    {
        var query = "SELECT * FROM CustomWorkflows.vCustomActionAccess";
        var customActions = _connection.Query<VCustomAction>(query);
        return customActions;
    }
}