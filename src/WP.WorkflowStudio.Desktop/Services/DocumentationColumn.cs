namespace WP.WorkflowStudio.Desktop.Services;

public class DocumentationColumn
{
    public int? id { get; set; }
    public int? table_id { get; set; }
    public string? column_name { get; set; }
    public string? type { get; set; }
    public int? is_nullable { get; set; }
    public string? description { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
}