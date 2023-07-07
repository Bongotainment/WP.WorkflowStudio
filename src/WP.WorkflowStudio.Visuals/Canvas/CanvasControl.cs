using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using WP.WorkflowStudio.Core.Models;
using WP.WorkflowStudio.Visuals.Canvas.Layers;
using WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

namespace WP.WorkflowStudio.Visuals.Canvas;

public class CanvasControl : UserControl
{
    public delegate void ElementClickedEventHandler(object sender, ElementClickedEventArgs e);

    public static readonly DirectProperty<CanvasControl, ICommand?> ElementClickedCommandProperty =
        AvaloniaProperty.RegisterDirect<CanvasControl, ICommand?>(nameof(ElementClickedCommand),
            o => o.ElementClickedCommand, (o, v) => o.ElementClickedCommand = v);


    private readonly CanvasModel _canvasModel;
    private readonly DebugInformationBaseLayer _debugInformationBaseLayer;
    private readonly EventFlowLayer _eventFlowLayer;
    private readonly SelectionLayer _selectionLayer;

    private ICommand? _elementClickedCommand;
    private bool _isResizing;

    public CanvasControl()
    {
        BorderBrush = Brush.Parse("#252526");
        BorderThickness = new Thickness(0);

        _canvasModel = new CanvasModel();
        _debugInformationBaseLayer = new DebugInformationBaseLayer(_canvasModel, Bounds);
        _eventFlowLayer = new EventFlowLayer(_canvasModel, Bounds);
        _selectionLayer = new SelectionLayer(_canvasModel, Bounds);
        _canvasModel.Zoom = 1f;

        // IBrush b = Brush.Parse("#252526");
        Background = Brushes.Transparent;

        PointerMoved += OnPointerMoved;

        PointerPressed += DrawControl_PointerPressed;
        PointerReleased += DrawControl_PointerReleased;
        // PointerMoved += DrawControl_PointerMoved;
        PointerWheelChanged += DrawControl_PointerWheelChanged;
        DoubleTapped += DrawControl_DoubleTapped;
    }

    public ICommand? ElementClickedCommand
    {
        get => _elementClickedCommand;
        set => SetAndRaise(ElementClickedCommandProperty, ref _elementClickedCommand, value);
    }

    private void InitializeColors()
    {
        if (LineColors is { Length: > 1 } && LineColors[0] != null && LineColors[1] != null)
        {
            SKColor[] arr = { LineColors[0]!.Value.ToSKColor(), LineColors[1]!.Value.ToSKColor() };
            ControlColors.LineColors = arr;
        }

        ControlColors.CanvasBackground = CanvasBackground?.ToSKColor() ?? ControlColors.CanvasBackground;
        ControlColors.CanvasGridColor = CanvasGridColor?.ToSKColor() ?? ControlColors.CanvasGridColor;
        ControlColors.EventBackground = EventBackground?.ToSKColor() ?? ControlColors.EventBackground;
        ControlColors.EventFontColor = EventFontColor?.ToSKColor() ?? ControlColors.EventFontColor;
        ControlColors.DetailLineColors = DetailLineColors?.ToSKColor() ?? ControlColors.DetailLineColors;
        ControlColors.ConnectionLineColor = ConnectionLineColor?.ToSKColor() ?? ControlColors.ConnectionLineColor;
        ControlColors.StandardFontColor = StandardFontColor?.ToSKColor() ?? ControlColors.StandardFontColor;
        ControlColors.ShadowColor = ShadowColor?.ToSKColor() ?? ControlColors.ShadowColor;
        ControlColors.LineDotColorStart = LineDotColorStart?.ToSKColor() ?? ControlColors.LineDotColorStart;
        ControlColors.LineDotColorEnd = LineDotColorEnd?.ToSKColor() ?? ControlColors.LineDotColorEnd;
        ControlColors.HasCustomConditionColor = HasCustomConditionColor?.ToSKColor() ??
                                                ControlColors.HasCustomConditionColor;
        ControlColors.IsCustomWorkflowColor = IsCustomWorkflowColor?.ToSKColor() ??
                                              ControlColors.IsCustomWorkflowColor;
        ControlColors.WorkflowInactiveColor = WorkflowInactiveColor?.ToSKColor() ??
                                              ControlColors.WorkflowInactiveColor;
        ControlColors.TextColor = TextColor?.ToSKColor() ?? ControlColors.TextColor;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this).ToSKPoint();
        _canvasModel.Coordinates = pos;

        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
        {
            if (_isResizing && _canvasModel.SelectedElement != null)
            {
                var x = pos.X + _canvasModel.SelectedElement.Size.Width - _canvasModel.ResizeStartClick.X -
                        GetCalculatedZoomTranslate().X; // / _canvasModel._zoom;
                var y = pos.Y + _canvasModel.SelectedElement.Size.Height - _canvasModel.ResizeStartClick.Y -
                        GetCalculatedZoomTranslate().Y; // / _canvasModel._zoom;
                // _canvasModel.SelectedElement.Size = new SKSize(x, y);
                _canvasModel.SelectedElement.SetSize(new SKSize(x, y));

                _canvasModel.ResizeStartClick = new SKPoint(pos.X - _canvasModel._matrix.TransX,
                    pos.Y - _canvasModel._matrix.TransY);
            }
            else if (_canvasModel.DraggedElement != null)
            {
                Cursor = Cursor.Parse("SizeAll");
                _canvasModel.DraggedElement.Move((pos.X - GetCalculatedZoomTranslate().X) / _canvasModel.Zoom,
                    (pos.Y - GetCalculatedZoomTranslate().Y) / _canvasModel.Zoom);
            }
        }

        if (point.Properties.IsRightButtonPressed)
        {
        }

        if (point.Properties.IsMiddleButtonPressed)
        {
            _canvasModel._matrix.TransX = pos.X - _canvasModel.TranslationStartClick.X;
            _canvasModel._matrix.TransY = pos.Y - _canvasModel.TranslationStartClick.Y;
        }

        var posTranslate = new SKPoint((pos.X - GetCalculatedZoomTranslate().X) / _canvasModel.Zoom,
            (pos.Y - GetCalculatedZoomTranslate().Y) / _canvasModel.Zoom);
        _selectionLayer.SetPosition(posTranslate);

        _debugInformationBaseLayer.SetPosition(pos);
        InvalidateVisual();
    }

    private void DrawControl_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        TappedEventArgs? args = null;
        if (e.GetType() != typeof(TappedEventArgs)) return;

        args = e as TappedEventArgs;
        var point = args?.GetPosition(this);
        //Todo
    }

    private SKPoint GetCalculatedZoomTranslate()
    {
        return new SKPoint(_canvasModel._matrix.TransX, _canvasModel._matrix.TransY);
    }

    private void DrawControl_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (_canvasModel.Zoom - 0.01 > _canvasModel.MinZoom && e.Delta.Y < 0) _canvasModel.Zoom -= 0.01f;

        if (_canvasModel.Zoom + 0.01 < _canvasModel.MaxZoom && e.Delta.Y > 0) _canvasModel.Zoom += 0.01f;


        var mouse = e.GetPosition(this).ToSKPoint();

        _canvasModel._matrix = _canvasModel._matrix.PostConcat(SKMatrix.CreateTranslation(-mouse.X, -mouse.Y));
        _canvasModel._matrix = _canvasModel._matrix.PostConcat(SKMatrix.CreateScale(
            _canvasModel.Zoom / _canvasModel._matrix.ScaleX, _canvasModel.Zoom / _canvasModel._matrix.ScaleY));
        _canvasModel._matrix = _canvasModel._matrix.PostConcat(SKMatrix.CreateTranslation(mouse.X, mouse.Y));
        InvalidateVisual();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        var texts = DataContext as ObservableCollection<WorkflowEvent>;
        EventView? lastItem = null;
        _canvasModel.SelectedElement = null;
        _canvasModel.ResetEventViews();
        if (texts == null) return;

        var squares = _canvasModel.GetEventViews();
        foreach (var item in texts)
        {
            if (item == null) continue;

            if (squares.Find(x => x.Id == item.EventId && x.CategoryId == item.ObjectId) != null) continue;

            lastItem = CreateEvent(lastItem, item);
        }

        var random = new Random();

        foreach (var square in squares) square.SetPosition(new SKPoint(random.Next(500), random.Next(500)));

        _eventFlowLayer.Run(1000);
        InvalidateVisual();
    }

    private EventView CreateEvent(EventView? lastItem, WorkflowEvent item)
    {
        if (item == null || item.Name == null)
            throw new InvalidOperationException("The provided Workflow was invalid.");

        var eventView = new EventView(0, 0, 0, item.Name, item.EventId, _canvasModel.ImageLoader);
        eventView.CategoryId = item.ObjectId;

        if (item.Color != null)
            eventView.BorderColor = new SKColor(item.Color.Value.R, item.Color.Value.G, item.Color.Value.B,
                item.Color.Value.A);

        if (lastItem != null)
        {
            eventView.SetPosition(lastItem.Size.Width + lastItem.DockingPointRight().X,
                lastItem.DockingPointTop().Y, 1f);
            var width = Bounds.Size.Width / _canvasModel.Zoom;
            if (eventView.DockingPointRight().X > width)
                eventView.SetPosition(0f, lastItem.DockingPointBottom().Y + 10f, 1f);
        }

        _canvasModel.AddEventView(eventView);
        lastItem = eventView;

        foreach (var flow in item.GetWorkflows())
        {
            eventView.CreateChild(flow.Name ?? string.Empty, flow.WorkflowId, flow.HasCustomCondition,
                flow.WasExecutedInPast, flow.IsCustomWorkflow);
            var view = eventView.GetChildren().First(x => x.Id == flow.WorkflowId);
            view.Activated = flow.Activated;
            view.ExecuteDelayed = flow.ExecuteDelayed;
            view.Index = flow.Position;
            foreach (var evt in flow.GetConnections())
            {
                var squares = _canvasModel.GetEventViews();
                if (squares.Find(x => x.Id == evt.GetId()) == null)
                {
                    var texts =
                        DataContext as ObservableCollection<WorkflowEvent>;
                    var newEvent = texts?.ToList()
                        .Find(x => x.EventId == evt.GetId() && x.ObjectId == evt.GetObjectId());
                    if (newEvent != null) lastItem = CreateEvent(lastItem, newEvent);
                }

                squares = _canvasModel.GetEventViews();
                var childEventView = squares.First(x => x.Id == evt.GetId());
                view.AddChild(childEventView);
            }
        }

        return lastItem;
    }

    private void DrawControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (!point.Properties.IsLeftButtonPressed)
        {
            _isResizing = false;
            if (_canvasModel.DraggedElement != null) _canvasModel.DraggedElement.SetClickPosition(0, 0);

            Cursor = Cursor.Default;
            _canvasModel.DraggedElement = null;
        }
    }

    private void DrawControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
        {
            HandleLeftPointerPressed(e);
        }
        else if (point.Properties.IsRightButtonPressed)
        {
        }
        else if (point.Properties.IsMiddleButtonPressed)
        {
            _canvasModel.TranslationStartClick =
                new SKPoint((float)(e.GetPosition(this).X - _canvasModel._matrix.TransX),
                    (float)(e.GetPosition(this).Y - _canvasModel._matrix.TransY));
        }
    }

    private void HandleLeftPointerPressed(PointerPressedEventArgs e)
    {
        if (_selectionLayer.CanResize && _canvasModel.SelectedElement != null)
        {
            _canvasModel.ResizeStartClick = new SKPoint((float)(e.GetPosition(this).X - _canvasModel._matrix.TransX),
                (float)(e.GetPosition(this).Y - _canvasModel._matrix.TransY));
            _isResizing = true;
        }

        var squares = _canvasModel.GetEventViews();
        foreach (var searchItem in squares)
        {
            var o = new Point(e.GetPosition(this).X - GetCalculatedZoomTranslate().X,
                e.GetPosition(this).Y - GetCalculatedZoomTranslate().Y);
            if (searchItem.IsClicked(o, _canvasModel.Zoom))
            {
                _canvasModel.DraggedElement = searchItem;
                _canvasModel.SelectedElement = searchItem;
                if (_canvasModel.DraggedElement != null)
                {
                    var foundItem = searchItem.GetChildren().FirstOrDefault(x => x.IsClicked(o, _canvasModel.Zoom));
                    if (foundItem != null)
                        RaiseElementClickedEvent(foundItem);

                    _canvasModel.DraggedElement.SetClickPosition(
                        (float)(e.GetPosition(this).X - GetCalculatedZoomTranslate().X) / _canvasModel.Zoom,
                        (float)(e.GetPosition(this).Y - GetCalculatedZoomTranslate().Y) / _canvasModel.Zoom);
                }

                return;
            }
        }

        if (!_isResizing) _canvasModel.SelectedElement = null;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var width = Bounds.Size.Width;
        var height = Bounds.Size.Height;
        if (width <= 0) return;


        if (height <= 0) return;

        _debugInformationBaseLayer.Bounds = Bounds;
        _eventFlowLayer.Bounds = Bounds;
        _selectionLayer.Bounds = Bounds;
        InitializeColors();

        var customDrawOp = new CustomDrawOp(Bounds, _canvasModel, _debugInformationBaseLayer, _eventFlowLayer,
            _selectionLayer);

        context.Custom(customDrawOp);
        Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Render);
        Cursor = _canvasModel.CurrentCursor;
    }

    public event ElementClickedEventHandler? ElementClickedEvent;

    private void RaiseElementClickedEvent(FlowView clickedElement)
    {
        var args = new ElementClickedEventArgs(clickedElement.ParentEventname,
            "NaN", clickedElement.Text,
            clickedElement.IsCustomWorkflow,
            clickedElement.WasExecutedInPast,
            clickedElement.HasCustomCondition,
            clickedElement.ExecuteDelayed,
            clickedElement.Activated
        );
        ElementClickedEvent?.Invoke(this, args);

        OnElementClicked(args);
    }

    protected virtual void OnElementClicked(ElementClickedEventArgs e)
    {
        ElementClickedCommand?.Execute(e);
    }

    #region Styles

    public static readonly StyledProperty<Color?> CanvasBackgroundProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(CanvasBackground));

    public Color? CanvasBackground
    {
        get => GetValue(CanvasBackgroundProperty);
        set => SetValue(CanvasBackgroundProperty, value);
    }

    public static readonly StyledProperty<Color?> CanvasGridColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(CanvasGridColor));

    public Color? CanvasGridColor
    {
        get => GetValue(CanvasGridColorProperty);
        set => SetValue(CanvasGridColorProperty, value);
    }

    public static readonly StyledProperty<Color?> EventBackgroundProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(EventBackground));

    public Color? EventBackground
    {
        get => GetValue(EventBackgroundProperty);
        set => SetValue(EventBackgroundProperty, value);
    }

    public static readonly StyledProperty<Color?> EventFontColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(EventFontColor));

    public Color? EventFontColor
    {
        get => GetValue(EventFontColorProperty);
        set => SetValue(EventFontColorProperty, value);
    }

    public static readonly StyledProperty<Color?> DetailLineColorsProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(DetailLineColors));

    public Color? DetailLineColors
    {
        get => GetValue(DetailLineColorsProperty);
        set => SetValue(DetailLineColorsProperty, value);
    }

    public static readonly StyledProperty<Color?> ConnectionLineColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(ConnectionLineColor));

    public Color? ConnectionLineColor
    {
        get => GetValue(ConnectionLineColorProperty);
        set => SetValue(ConnectionLineColorProperty, value);
    }

    public static readonly StyledProperty<Color?> StandardFontColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(StandardFontColor));

    public Color? StandardFontColor
    {
        get => GetValue(StandardFontColorProperty);
        set => SetValue(StandardFontColorProperty, value);
    }

    public static readonly StyledProperty<Color?> ShadowColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(ShadowColor));

    public Color? ShadowColor
    {
        get => GetValue(ShadowColorProperty);
        set => SetValue(ShadowColorProperty, value);
    }

    public static readonly StyledProperty<Color?[]> LineColorsProperty =
        AvaloniaProperty.Register<CanvasControl, Color?[]>(nameof(LineColors));

    public Color?[] LineColors
    {
        get => GetValue(LineColorsProperty);
        set => SetValue(LineColorsProperty, value);
    }

    public static readonly StyledProperty<Color?> LineDotColorStartProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(LineDotColorStart));

    public Color? LineDotColorStart
    {
        get => GetValue(LineDotColorStartProperty);
        set => SetValue(LineDotColorStartProperty, value);
    }

    public static readonly StyledProperty<Color?> LineDotColorEndProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(LineDotColorEnd));

    public Color? LineDotColorEnd
    {
        get => GetValue(LineDotColorEndProperty);
        set => SetValue(LineDotColorEndProperty, value);
    }

    public static readonly StyledProperty<Color?> HasCustomConditionColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(HasCustomConditionColor));

    public Color? HasCustomConditionColor
    {
        get => GetValue(HasCustomConditionColorProperty);
        set => SetValue(HasCustomConditionColorProperty, value);
    }

    public static readonly StyledProperty<Color?> IsCustomWorkflowColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(IsCustomWorkflowColor));

    public Color? IsCustomWorkflowColor
    {
        get => GetValue(IsCustomWorkflowColorProperty);
        set => SetValue(IsCustomWorkflowColorProperty, value);
    }

    public static readonly StyledProperty<Color?> WorkflowInactiveColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(WorkflowInactiveColor));

    public Color? WorkflowInactiveColor
    {
        get => GetValue(WorkflowInactiveColorProperty);
        set => SetValue(WorkflowInactiveColorProperty, value);
    }

    public static readonly StyledProperty<Color?> TextColorProperty =
        AvaloniaProperty.Register<CanvasControl, Color?>(nameof(TextColor));

    public Color? TextColor
    {
        get => GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    #endregion
}