using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using WP.WorkflowStudio.Desktop.ViewModels;

namespace WP.WorkflowStudio.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel model)
    {
        DataContext = model;
        InitializeComponent();

        panelLeft.PointerPressed += PanelOnPointerPressed;
        panelRight.PointerPressed += PanelOnPointerPressed;
        panelLeft.DoubleTapped += DoubleTabbed;
        panelRight.DoubleTapped += DoubleTabbed;
    }

    private MainWindowViewModel? GetViewModel()
    {
        return (MainWindowViewModel?)DataContext;
    }

    private void PanelOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void DoubleTabbed(object? sender, RoutedEventArgs e)
    {
        if (WindowState != WindowState.Maximized)
            WindowState = WindowState.Maximized;
        else
            WindowState = WindowState.Normal;
    }

    private async void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        var viewModel = GetViewModel();
        if (viewModel != null) await viewModel.ShowLoginDialog();

        Focus();
    }
}