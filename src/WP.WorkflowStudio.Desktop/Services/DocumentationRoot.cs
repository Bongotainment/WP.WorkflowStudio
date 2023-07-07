using System;
using System.Collections.Generic;

namespace WP.WorkflowStudio.Desktop.Services;

public class DocumentationRoot
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<DocumentationEntry> Tables { get; set; } = new();
}