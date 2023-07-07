using System;

namespace WP.WorkflowStudio.Desktop.Views.Menu;

public class MenuButtonClickedEventArgs : EventArgs
{
    public int CategoryId { get; set; }
}