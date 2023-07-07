using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override TWorkflow Get<TWorkflow>(int workflowId)
    {
        var query = "SELECT * FROM tWorkflow WHERE œkWorkflow";
        var parameters = new Tuple<string, object>("œkWorkflow", workflowId);
        var workflow = _connection.Query<TWorkflow>(query, parameters).First();
        return workflow;
    }

    public override IEnumerable<TWorkflow> Get<TWorkflow>()
    {
        var workflows = new List<TWorkflow>();
        var query = "SELECT * FROM tWorkflow";
        workflows = _connection.Query<TWorkflow>(query);
        return workflows;
    }
}