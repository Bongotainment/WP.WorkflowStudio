using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.DBO;

public class RMStatusWorkflowAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public RMStatusWorkflowAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TRmStatusWorkflow> Get<TRmStatusWorkflow>()
    {
        var query = "SELECT * FROM tRMStatusWorkflow";
        IEnumerable<TRmStatusWorkflow> rmstatusWorkflow = _connection.Query<TRmStatusWorkflow>(query);
        return rmstatusWorkflow;
    }
}