using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.CustomWorkflows;

public class AllowedDataTypesAccess
{
    private readonly SQLServerConnection _connection;

    public AllowedDataTypesAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public List<TAllowedDatatypes> Get()
    {
        var query = "SELECT * FROM CustomWorkflows.tAllowedDatatypes";
        var allowedDatatypesList = _connection.Query<TAllowedDatatypes>(query);
        return allowedDatatypesList;
    }
}