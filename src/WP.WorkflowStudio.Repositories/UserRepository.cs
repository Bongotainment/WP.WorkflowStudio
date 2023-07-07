using System.Security.Cryptography;
using System.Text;
using WP.WorkflowStudio.DataAccess.DBO;

namespace WP.WorkflowStudio.Repositories;

public class UserRepository
{
    private readonly BenutzerAccess _useraccess;

    public UserRepository(BenutzerAccess useraccess)
    {
        _useraccess = useraccess;
    }

    [Obsolete]
    public Task<bool> CheckUserCredentials(string username, string pass, string mandant)
    {
        bool result;
        try
        {
            var user = _useraccess.Get<Tbenutzer>(username);
            var salt = user.ISalt.ToString().ToUpper();
            var password = salt + "JTL" + pass;

            // Use input string to calculate MD5 hash
            using (var sha1 = SHA1.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(password);
                var hashBytes = sha1.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("X2"));

                var passwordHash = user.CPasswort;
                if (sb.Equals(passwordHash))
                    result = true;
                else
                    result = false;
            }
        }
        catch (Exception)
        {
            result = false;
        }

        return Task.FromResult(result);
    }
}