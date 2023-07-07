using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.DBO;

public class RMStatusSpracheAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public RMStatusSpracheAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TRmStatusSprache> Get<TRmStatusSprache>()
    {
        var query = "SELECT * FROM tRMStatusSprache";
        IEnumerable<TRmStatusSprache> rmStatusSprache = _connection.Query<TRmStatusSprache>(query);
        return rmStatusSprache;
    }
}