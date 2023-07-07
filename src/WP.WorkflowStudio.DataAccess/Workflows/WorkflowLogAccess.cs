using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowLogAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowLogAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }


    public override IEnumerable<TWorkflowLog> Get<TWorkflowLog>()
    {
        var result = new List<TWorkflowLog>();
        var query = "SELECT * FROM tWorkflowLog";
        result = _connection.Query<TWorkflowLog>(query);
        return result;
    }

    public IEnumerable<TWorkflowLog> GetMiniGrouped<TWorkflowLog>()
    {
        var result = new List<TWorkflowLog>();
        var query = "select kWorkflow, MAX(dDatum) as dDatum from tWorkflowLog GROUP BY kWorkflow";
        result = _connection.Query<TWorkflowLog>(query);
        return result;
    }
}