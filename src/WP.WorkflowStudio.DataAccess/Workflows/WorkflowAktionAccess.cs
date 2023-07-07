using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.Workflows;

public class WorkflowAktionAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public WorkflowAktionAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override IEnumerable<TWorkflowAktion> Get<TWorkflowAktion>()
    {
        var result = new List<TWorkflowAktion>();
        var query = "SELECT * FROM tWorkflowAktion";
        result = _connection.Query<TWorkflowAktion>(query);
        return result;
    }
}