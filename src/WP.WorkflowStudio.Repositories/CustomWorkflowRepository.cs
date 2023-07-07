using WP.WorkflowStudio.DataAccess.CustomWorkflows;

namespace WP.WorkflowStudio.Repositories;

public class CustomWorkflowRepository
{
    private readonly AllowedDataTypesAccess _allowedDataTypesAccess;
    private readonly CustomActionAccess _customActionAccess;
    private readonly WorkflowObjectsAccess _workflowObjectsAccess;

    public CustomWorkflowRepository(AllowedDataTypesAccess allowedDataTypesAccess,
        CustomActionAccess customActionAccess, WorkflowObjectsAccess workflowObjectsAccess)
    {
        _customActionAccess = customActionAccess;
        _workflowObjectsAccess = workflowObjectsAccess;
        _allowedDataTypesAccess = allowedDataTypesAccess;
    }

    public Task<List<string>> GetAllowedDatatypes()
    {
        var allowedDatatypes = _allowedDataTypesAccess.Get().Select(x => x.TypeName).ToList();
        return Task.FromResult(allowedDatatypes);
    }

    public Task<object> GetWorkflowObjects()
    {
        // var allowedDatatypes = _workflowObjectsAccess.Get();
#pragma warning disable CS8625
        return Task.FromResult<object>(null);
#pragma warning restore CS8625
    }
}