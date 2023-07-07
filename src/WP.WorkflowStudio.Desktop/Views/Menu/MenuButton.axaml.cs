using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace WP.WorkflowStudio.Desktop.Views.Menu;

public partial class MenuButton : UserControl
{
    public MenuButton()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        Text = "Lorem Ipsum";
        ShortCut = "F2";
        Color = Brushes.Crimson;
        ImagePath = "/Assets/images/shoppingBag.svg";
        ImgMargin = new Thickness(5);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var args = new MenuButtonClickedEventArgs
        {
            CategoryId = CategoryId
        };
        OnMenuButtonClicked(args);
    }

    #region Property Text

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<MenuButton, string>(nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region Property Color

    public static readonly StyledProperty<IBrush?> ColorProperty =
        AvaloniaProperty.Register<MenuButton, IBrush?>(nameof(Color));

    public IBrush? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    #endregion

    #region ShortCut

    public static readonly StyledProperty<string> ShortCutProperty =
        AvaloniaProperty.Register<MenuButton, string>(nameof(ShortCut));

    public string ShortCut
    {
        get => GetValue(ShortCutProperty);
        set => SetValue(ShortCutProperty, value);
    }

    #endregion

    #region IamgePath

    public static readonly StyledProperty<string> ImagePathProperty =
        AvaloniaProperty.Register<MenuButton, string>(nameof(ImagePath));

    public string ImagePath
    {
        get => GetValue(ImagePathProperty);
        set => SetValue(ImagePathProperty, value);
    }

    #endregion

    #region IamgePath

    public static readonly StyledProperty<Thickness> ImgMarginProperty =
        AvaloniaProperty.Register<MenuButton, Thickness>(nameof(ImgMargin));

    public Thickness ImgMargin
    {
        get => GetValue(ImgMarginProperty);
        set => SetValue(ImgMarginProperty, value);
    }

    #endregion

    #region ClickAndCommand

    protected virtual void OnMenuButtonClicked(MenuButtonClickedEventArgs e)
    {
        MenuButtonClickedCommand?.Execute(e);
    }

    public static readonly DirectProperty<MenuButton, ICommand?> MenuButtonClickedCommandProperty =
        AvaloniaProperty.RegisterDirect<MenuButton, ICommand?>(nameof(MenuButtonClickedCommand),
            o => o.MenuButtonClickedCommand, (o, v) => o.MenuButtonClickedCommand = v);

    private ICommand? _menuButtonClickedCommand;

    public ICommand? MenuButtonClickedCommand
    {
        get => _menuButtonClickedCommand;
        set => SetAndRaise(MenuButtonClickedCommandProperty, ref _menuButtonClickedCommand, value);
    }

    #endregion

    #region Property CategoryId

    public static readonly StyledProperty<int> CategoryIdProperty =
        AvaloniaProperty.Register<MenuButton, int>(nameof(CategoryId));

    public int CategoryId
    {
        get => GetValue(CategoryIdProperty);
        set => SetValue(CategoryIdProperty, value);
    }

    #endregion
}