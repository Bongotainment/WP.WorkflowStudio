using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;

public abstract class BaseTreeViewElement : ObservableValidator //ObservableObject
{
    public abstract string TreeViewDisplayName { get; }

    public Guid Identity { get; protected init; }

    public event EventHandler? ObjectChanged;

    protected void OnObjectChanged()
    {
        ObjectChanged?.Invoke(this, EventArgs.Empty);
    }
}