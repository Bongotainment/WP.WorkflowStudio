using Avalonia;
using Avalonia.Controls;

namespace WP.WorkflowStudio.Desktop.Views.CustomWorkflows.Dialogs;

public partial class AddProcedureDialog : Window
{
    public AddProcedureDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    // private void InitializeComponent()
    // {
    //     AvaloniaXamlLoader.Load(this);
    // }
}