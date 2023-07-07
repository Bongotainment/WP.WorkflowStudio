using WP.WorkflowStudio.Core.Interfaces;
using WP.WorkflowStudio.DataAccess.SQLServer;

namespace WP.WorkflowStudio.DataAccess.DBO;

public class BenutzerAccess : BaseDataAccess
{
    private readonly SQLServerConnection _connection;

    public BenutzerAccess(SQLServerConnection connection)
    {
        _connection = connection;
    }

    public override Tbenutzer Get<Tbenutzer>(int userid)
    {
        var query = "SELECT * FROM tbenutzer WHERE kWorkflow = œkWorkflow";
        var parameters = new Tuple<string, object>("@kbenutzer", userid);
        var user = _connection.Query<Tbenutzer>(query, parameters).First();
        return user;
    }

    public Tbenutzer Get<Tbenutzer>(string username)
    {
        var query = "SELECT * FROM tbenutzer WHERE cLogin = @cLogin";
        var parameters = new Tuple<string, object>("@cLogin", username);
        var user = _connection.Query<Tbenutzer>(query, parameters).First();
        return user;
    }


    public IEnumerable<Tbenutzer> Get()
    {
        var query = "SELECT * FROM tbenutzer";
        IEnumerable<Tbenutzer> users = _connection.Query<Tbenutzer>(query);
        return users;
    }
}