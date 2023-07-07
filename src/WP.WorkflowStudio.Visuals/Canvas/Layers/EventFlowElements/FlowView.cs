using WP.WorkflowStudio.Visuals.Canvas.Utilities;

namespace WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

internal class FlowView : BaseViewElement
{
    private readonly float _descent;
    private readonly ImageLoader _imageLoader;
    private readonly List<EventView> _mChildren;
    private readonly SKSize _textSize;

    public FlowView(int id, ImageLoader imageLoader)
    {
        Text = string.Empty;
        ParentEventname = string.Empty;
        _mChildren = new List<EventView>();
        PaddingRight = 10f;
        PaddingLeft = 10f;
        PaddingTop = 3f;
        PaddingBottom = 3f;
        Id = id;
        _imageLoader = imageLoader;
    }

    public FlowView(string text, int id, bool wasExecutedInPast, ImageLoader imageLoader) : this(id, imageLoader)
    {
        Text = text;
        using (var fontPaint = new SKPaint())
        {
            fontPaint.TextSize = 12.0f;
            fontPaint.IsAntialias = true;
            fontPaint.Color = new SKColor(0x42, 0x81, 0xA4);
            fontPaint.IsStroke = false;

            var textBounds = new SKRect();
            fontPaint.MeasureText(text, ref textBounds);
            _descent = fontPaint.FontMetrics.Descent;

            textBounds.Bottom = fontPaint.FontMetrics.Bottom;
            _textSize = new SKSize(textBounds.Size.Width, textBounds.Size.Height);
            var width = _textSize.Width + PaddingLeft + PaddingRight;
            var height = _textSize.Height + PaddingTop + PaddingBottom;


            width += height; //We may draw images left of the text in the size of the height of the flowview
            if (!wasExecutedInPast)
                //if the flow wasn't executed in the past the snowflake image is shown.
                width += height;

            WasExecutedInPast = wasExecutedInPast;

            Size = new SKSize(width, height);
        }
    }

    public string ParentEventname { get; set; } //maybe give the flow a parent object?

    public bool Activated { get; set; }
    public bool HasCustomCondition { get; set; }

    public string Text { get; set; }

    public int Id { get; }

    public int Index { get; set; }

    public bool ExecuteDelayed { get; set; }

    public bool WasExecutedInPast { get; set; }

    public bool IsCustomWorkflow { get; set; }

    public void Move(float x, float y)
    {
        Position = new SKPoint(x, y);
    }

    public void Draw(SKCanvas canvas)
    {
        DrawText(canvas);
    }

    private void DrawText(SKCanvas canvas)
    {
        var x = Position.X + PaddingLeft + Size.Height;
        var y = Position.Y + Size.Height - (PaddingBottom + _descent);
        using (var fontPaint = new SKPaint())
        {
            fontPaint.TextSize = 12.0f;
            fontPaint.IsAntialias = true;
            fontPaint.Color = ControlColors.EventFontColor;

            var colors = new List<SKColor>();
            if (ExecuteDelayed) DrawClock(canvas);

            if (HasCustomCondition) colors.Add(ControlColors.HasCustomConditionColor);
            if (IsCustomWorkflow) colors.Add(ControlColors.IsCustomWorkflowColor);

            fontPaint.Color = ControlColors.TextColor;
            var colorBreakPoint = 1.0f / colors.Count;
            var colorPos = Enumerable.Repeat(colorBreakPoint, colors.Count).ToList();

            var shader = SKShader.CreateLinearGradient(
                new SKPoint(Position.X, Position.Y),
                new SKPoint(Position.X + Size.Width, Position.Y + Size.Height),
                colors.ToArray(),
                colorPos.ToArray(),
                SKShaderTileMode.Clamp);
            fontPaint.Shader = shader;

            if (!Activated)
            {
                fontPaint.Shader = null;
                fontPaint.Color = ControlColors.WorkflowInactiveColor;
            }

            if (!WasExecutedInPast)
            {
                float padding = 0;
                if (ExecuteDelayed)
                {
                    padding += PaddingLeft + 2;
                    x += PaddingLeft;
                }

                DrawSnowflake(canvas, padding);
            }

            fontPaint.IsStroke = false;
            fontPaint.TextAlign = SKTextAlign.Left;
            canvas.DrawText(Text, x, y, fontPaint);
        }
    }

    private void DrawClock(SKCanvas canvas)
    {
        var svg = _imageLoader.GetImageByName("clock.svg");
        DrawSVGOnCanvas(canvas, 0, svg);
    }

    private void DrawSnowflake(SKCanvas canvas, float paddingLeft)
    {
        var svg = _imageLoader.GetImageByName("freeze.svg");
        DrawSVGOnCanvas(canvas, paddingLeft, svg);
    }

    private void DrawSVGOnCanvas(SKCanvas canvas, float paddingLeft, SKSvg svg)
    {
        // Draw the SVG image onto the canvas
        using (var paint = new SKPaint())
        {
            var picture = svg.Picture;
            paint.FilterQuality = SKFilterQuality.High;
            var height = _textSize.Height; //this.Size.Height

            var matrix = SKMatrix.CreateScale(height / picture.CullRect.Width, height / picture.CullRect.Height);
            matrix.TransX = -picture.CullRect.Left * matrix.ScaleX + Position.X + PaddingLeft + paddingLeft;
            matrix.TransY = -picture.CullRect.Top * matrix.ScaleY + Position.Y + PaddingTop;

            paint.ColorFilter = SKColorFilter.CreateBlendMode(ControlColors.TextColor, SKBlendMode.SrcIn);
            canvas.DrawPicture(picture, ref matrix, paint);
        }
    }

    private void DrawBorder(SKCanvas canvas)
    {
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;
            paint.Color = ControlColors.TextColor;
            paint.StrokeCap = SKStrokeCap.Round;
            paint.StrokeWidth = 1f;
            paint.Style = SKPaintStyle.Stroke;

            var sizeX = Size.Width + Position.X;
            var sizeY = Size.Height + Position.Y;

            var rect = new SKRect(Position.X, Position.Y, sizeX, sizeY);
            canvas.DrawRect(rect, paint);
        }
    }

    public void AddChild(EventView view)
    {
        _mChildren.Add(view);
    }

    internal IEnumerable<EventView> GetChildren()
    {
        return _mChildren;
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
}