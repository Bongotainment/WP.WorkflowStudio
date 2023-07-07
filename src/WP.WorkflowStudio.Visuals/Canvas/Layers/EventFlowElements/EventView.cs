using WP.WorkflowStudio.Visuals.Canvas.Utilities;

namespace WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

internal class EventView : BaseViewElement
{
    private readonly List<FlowView> _childElements;
    private readonly EventViewHeader _header;
    private readonly ImageLoader _imageLoader;

    private float _xClicked;
    private float _yClicked;

    public EventView(float xPos, float yPos, float zPos, string text, int id, ImageLoader imageLoader)
    {
        MarginTop = 0;
        MarginLeft = 0;
        MarginRight = 0;
        MarginBottom = 0;

        PaddingRight = 0;
        PaddingLeft = 0;
        PaddingTop = 0;
        PaddingBottom = 0f;

        _childElements = new List<FlowView>();
        _header = new EventViewHeader(text);
        SetPosition(xPos, yPos, zPos);

        Id = id;
        _imageLoader = imageLoader;
        using (var fontPaint = new SKPaint())
        {
            fontPaint.TextSize = 12.0f;
            fontPaint.IsAntialias = true;
            fontPaint.Color = new SKColor(0x42, 0x81, 0xA4);
            fontPaint.IsStroke = false;

            var width = _header.Size.Width + Position.X + PaddingLeft + PaddingRight;
            var height = _header.Size.Height + Position.Y + PaddingTop + PaddingBottom;

            Size = new SKSize(width, height);
        }
    }


    public int Id { get; set; }

    public int CategoryId { get; set; }

    public SKColor BorderColor { get; set; } = ControlColors.ShadowColor;

    public bool HasConnectionToEvent(EventView? eventView)
    {
        if (eventView == null) return false;
        return _childElements.Any(x => x.GetChildren().Contains(eventView));
    }

    public void Move(float x, float y)
    {
        Position = new SKPoint(x + _xClicked, y + _yClicked);
        _header.Position = new SKPoint(Position.X + PaddingLeft, Position.Y + PaddingTop);
    }

    public void SetPosition(float xPos, float yPos, float zPos)
    {
        Position = new SKPoint(xPos, yPos);
        _header.Move(xPos + PaddingLeft, yPos + PaddingTop);
    }

    public void SetPosition(SKPoint position)
    {
        Position = position;
        _header.Move(position.X + PaddingLeft, position.Y + PaddingTop);
    }

    internal void SetClickPosition(float x, float y)
    {
        _xClicked = Position.X - x;
        _yClicked = Position.Y - y;
    }

    public bool IsClicked(Point point, float zoom)
    {
        var sizeX = Size.Width + Position.X;
        sizeX = sizeX * zoom;

        var sizeY = Size.Height + Position.Y;
        sizeY = sizeY * zoom;

        var rect = new SKRect(Position.X * zoom, Position.Y * zoom, sizeX, sizeY);
        if (point.X >= rect.Left && point.X <= rect.Right &&
            point.Y >= rect.Top && point.Y <= rect.Bottom)
            return true;

        return false;
    }

    public void Draw(SKCanvas canvas)
    {
        // draw left-aligned text, solid
        DrawBorder(canvas);
        DrawBackground(canvas);
        DrawHeaderRectangle(canvas);
        DrawChildren(canvas);
    }

    private void DrawBackground(SKCanvas canvas)
    {
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;
            paint.Color = ControlColors.EventBackground;
            paint.StrokeCap = SKStrokeCap.Round;
            paint.StrokeWidth = 2f;
            paint.Style = SKPaintStyle.Fill;

            var sizeX = Size.Width + Position.X;
            var sizeY = Size.Height + Position.Y;

            var rect = new SKRect(Position.X, Position.Y, sizeX, sizeY);
            var roundRect = new SKRoundRect(rect, 2.0f);
            canvas.DrawRoundRect(roundRect, paint);
        }
    }

    internal void CreateChild(string name, int id, bool hasCustomCondition, bool wasExecutedPast, bool isCustomWorkflow)
    {
        var flowSquare = new FlowView(name, id, wasExecutedPast, _imageLoader)
        {
            ParentEventname = _header.GetText(),
            HasCustomCondition = hasCustomCondition,
            IsCustomWorkflow = isCustomWorkflow
        };
        AddChild(flowSquare);
        var childCount = _childElements.Count() + 1;

        var maxHeight = _childElements.Max(x => x.Size.Height);
        Size = new SKSize(Size.Width, _header.Size.Height + maxHeight * childCount);

        if (Size.Width < flowSquare.Size.Width)
        {
            Size = new SKSize(flowSquare.Size.Width, Size.Height);
            _header.Size = new SKSize(flowSquare.Size.Width, _header.Size.Height);
        }
    }

    private void DrawHeaderRectangle(SKCanvas canvas)
    {
        _header.Draw(canvas);
    }

    public void DrawChildren(SKCanvas canvas)
    {
        var count = 0;
        var maxHeight = 0f;
        if (_childElements.Count > 0) maxHeight = _childElements.Max(x => x.Size.Height);
        foreach (var child in GetChildren())
        {
            var nextRowY = count * maxHeight + _header.Size.Height;
            child.Position = SKPoint.Add(Position, new SKPoint(0, nextRowY));
            child.Draw(canvas);
            count++;
        }
    }

    private void DrawBorder(SKCanvas canvas)
    {
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;
            paint.Color = BorderColor;
            paint.StrokeCap = SKStrokeCap.Square;
            paint.StrokeWidth = 1f;
            paint.Style = SKPaintStyle.Fill;

            paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 3);

            var sizeX = Size.Width + Position.X;
            var sizeY = Size.Height + Position.Y;

            var rect = new SKRect(Position.X, Position.Y, sizeX, sizeY);
            canvas.DrawRect(rect, paint);
        }
    }

    public void AddChild(FlowView flow)
    {
        _childElements.Add(flow);
    }

    public void SetSize(SKSize size)
    {
        Size = size;
        _header.Size = new SKSize(size.Width, _header.Size.Height);
    }

    public IEnumerable<FlowView> GetChildren()
    {
        return _childElements.OrderBy(x => x.Index);
    }
}