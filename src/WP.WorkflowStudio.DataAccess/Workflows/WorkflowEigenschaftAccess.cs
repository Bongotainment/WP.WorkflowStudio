using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowEigenschaftAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowEigenschaftAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TWorkflowEigenschaft> Get<TWorkflowEigenschaft>()
    {
        var result = new List<TWorkflowEigenschaft>();
        var query = "SELECT * FROM tWorkflowEigenschaft";
        result = _connection.Query<TWorkflowEigenschaft>(query);
        return result;
    }
}