using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowEventGroupAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowEventGroupAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }


    public override IEnumerable<TWorkflowEventGroup> Get<TWorkflowEventGroup>()
    {
        var result = new List<TWorkflowEventGroup>();
        var query = "SELECT * FROM tWorkflowEventGroup";
        result = _connection.Query<TWorkflowEventGroup>(query);
        return result;
    }
}