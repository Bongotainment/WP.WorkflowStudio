using WP.WorkflowStudio.Core.Models;
using WP.WorkflowStudio.DataAccess.DBO;

namespace WP.WorkflowStudio.Repositories;

public class VersionRepository
{
    private readonly JTLVersionAccess _versionAccess;

    public VersionRepository(JTLVersionAccess versionAccess)
    {
        _versionAccess = versionAccess;
    }

    public JtlVersion GetJtlVersion()
    {
        var versionnumber = _versionAccess.Get();
        var version = new JtlVersion(versionnumber);
        return version;
    }
}