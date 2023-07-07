using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowBedingungAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowBedingungAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TWorkflowBedingung> Get<TWorkflowBedingung>()
    {
        var result = new List<TWorkflowBedingung>();
        var query = "SELECT * FROM tWorkflowBedingung";
        result = _connection.Query<TWorkflowBedingung>(query);
        return result;
    }
}