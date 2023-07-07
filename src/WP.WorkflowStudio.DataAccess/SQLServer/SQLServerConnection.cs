using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace WP.WorkflowStudio.DataAccess.SQLServer;

public class SQLServerConnection
{
    public string? Address { get; set; }
    public string? Database { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }

    private string ConnectionString =>
        $"Server = {Address}; Database = {Database}; User Id = {User}; Password = {Password};";

    public async Task<bool> TestConnection()
    {
        var result = false;
        if (string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Database)) return result;

        try
        {
            await using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
            }

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }

        return result;
    }

    public List<T> Query<T>(string query, params Tuple<string, object>[] parameters)
    {
        var dynParameters = ParseArguments(parameters);
        using (var connection = new SqlConnection(ConnectionString))
        {
            if (dynParameters != null)
                return connection.Query<T>(query, dynParameters).ToList();
            return connection.Query<T>(query).ToList();
        }
    }

    private DynamicParameters? ParseArguments(Tuple<string, object>[] arguments)
    {
        if (arguments is null) return null;

        var parameters = new DynamicParameters();
        foreach (var tuple in arguments)
            if (tuple.Item2 is DateTime)
                parameters.Add(tuple.Item1, tuple.Item2, DbType.DateTime);
            else
                parameters.Add(tuple.Item1, tuple.Item2);
        return parameters;
    }
}