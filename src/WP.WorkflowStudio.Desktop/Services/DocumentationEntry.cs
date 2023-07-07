using System;
using System.Collections.Generic;

namespace WP.WorkflowStudio.Desktop.Services;

public class DocumentationEntry
{
    public int? Id { get; set; }
    public int? Version_Id { get; set; }
    public string? Object_Name { get; set; }
    public string? Schema_Name { get; set; }
    public string? Description { get; set; }
    public string? Long_Description { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public List<DocumentationColumn> Columns { get; set; } = new();
}