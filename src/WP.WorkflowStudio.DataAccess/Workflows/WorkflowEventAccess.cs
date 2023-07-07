using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowEventAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowEventAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TWorkflowEvent> Get<TWorkflowEvent>()
    {
        var result = new List<TWorkflowEvent>();
        var query = "SELECT * FROM tWorkflowEvent";
        result = _connection.Query<TWorkflowEvent>(query);
        return result;
    }
}