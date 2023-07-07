using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.DBO;

public class JTLVersionAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public JTLVersionAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public string Get()
    {
        var result = string.Empty;
        var versions = new List<string>();
        var query = "SELECT TOP 1 cVersion FROM tversion";
        versions = _connection.Query<string>(query);
        if (versions.Any()) result = versions.First();
        return result;
    }
}