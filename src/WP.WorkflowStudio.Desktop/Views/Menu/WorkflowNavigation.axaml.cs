using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace WP.WorkflowStudio.Desktop.Views.Menu;

public partial class WorkflowNavigation : UserControl
{
    public static readonly StyledProperty<string> CurrentTimeProperty =
        AvaloniaProperty.Register<MenuButton, string>(nameof(CurrentTime));

    public static readonly StyledProperty<string> CurrentDateProperty =
        AvaloniaProperty.Register<MenuButton, string>(nameof(CurrentDate));

    public static readonly StyledProperty<Dictionary<int, IBrush>> NavigationColorsProperty =
        AvaloniaProperty.Register<MenuButton, Dictionary<int, IBrush>>(nameof(NavigationColors));

    public WorkflowNavigation()
    {
        NavigationColors = new Dictionary<int, IBrush>();
        CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        CurrentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        InitializeComponent();
    }


    private string CurrentTime
    {
        get => GetValue(CurrentTimeProperty);
        set => SetValue(CurrentTimeProperty, value);
    }

    private string CurrentDate
    {
        get => GetValue(CurrentDateProperty);
        set => SetValue(CurrentDateProperty, value);

        // get => _currentDate;
        // set => SetAndRaise(CurrentDateProperty, ref _currentDate, value);
    }

    private Dictionary<int, IBrush> NavigationColors
    {
        get => GetValue(NavigationColorsProperty);
        set => SetValue(NavigationColorsProperty, value);

        // get => _navigationColors;
        // set => SetAndRaise(NavigationColorsProperty, ref _navigationColors, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, args) =>
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            CurrentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        };
        timer.Start();
    }
}