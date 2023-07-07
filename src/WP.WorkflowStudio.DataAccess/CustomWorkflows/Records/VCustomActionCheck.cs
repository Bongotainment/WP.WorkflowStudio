namespace WP.WorkflowStudio.DataAccess.CustomWorkflows.Records;

public class VCustomActionCheck
{
    public string CName { get; set; } = string.Empty;
    public string CDisplayName { get; set; } = string.Empty;
    public string CDescription { get; set; } = string.Empty;
    public string CObject { get; set; } = string.Empty;
    public int NObject { get; set; }
    public string CPkCol { get; set; } = string.Empty;
    public int NParamCount { get; set; }
    public string CNotAllowedParamTypesInAction { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}