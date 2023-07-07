namespace WP.WorkflowStudio.Core.Models;

public class Profile
{
    public string ProfileName { get; set; } = string.Empty;
    public List<string> MandantNames { get; init; } = new();
    public string LastUser { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}