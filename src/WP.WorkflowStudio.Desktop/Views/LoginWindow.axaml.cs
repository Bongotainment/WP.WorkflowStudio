using Avalonia;
using Avalonia.Controls;

namespace WP.WorkflowStudio.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
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