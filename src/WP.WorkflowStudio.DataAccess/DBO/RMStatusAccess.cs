using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.DBO;

public class RMStatusAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public RMStatusAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TRmStatus> Get<TRmStatus>()
    {
        var query = "SELECT * FROM tRMStatus";
        IEnumerable<TRmStatus> rmstatus = _connection.Query<TRmStatus>(query);
        return rmstatus;
    }
}