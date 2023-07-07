using System.Collections.Generic;
using System.Threading.Tasks;
using WP.WorkflowStudio.Core.Models;

namespace WP.WorkflowStudio.Desktop.Services;

public class ProfileService
{
    public Task<IEnumerable<Profile>> GetProfiles()
    {
        var dummyList = new List<Profile>();
        var profileStandard = new Profile
        {
            ProfileName = "Standard",
            LastUser = "admin",
            Address = "localhost",
            Database = "eazybusiness",
            User = "sa",
            Password = "!"
        };
        profileStandard.MandantNames.Add("eB-Standard");
        profileStandard.MandantNames.Add("Mein Unternehmen");
        dummyList.Add(profileStandard);
        IEnumerable<Profile> result = dummyList;
        return Task.FromResult(result);
    }
}