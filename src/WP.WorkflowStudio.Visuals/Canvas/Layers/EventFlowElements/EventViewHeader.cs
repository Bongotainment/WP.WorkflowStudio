namespace WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

internal class EventViewHeader : BaseViewElement
{
    private readonly string _mText;
    private float _ascent;
    private readonly float _descent;
    private float _leading;

    public EventViewHeader(string text)
    {
        MarginTop = 0;
        MarginLeft = 0;
        MarginRight = 0;
        MarginBottom = 0;

        PaddingTop = 10;
        PaddingLeft = 10;
        PaddingRight = 10;
        PaddingBottom = 10;

        _mText = text;
        using (var fontPaint = new SKPaint())
        {
            fontPaint.TextSize = 12.0f;
            fontPaint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright);
            fontPaint.FakeBoldText = true;
            fontPaint.IsAntialias = true;
            fontPaint.Color = new SKColor(0x42, 0x81, 0xA4);
            fontPaint.IsStroke = false;

            var textBounds = new SKRect();
            fontPaint.MeasureText(text, ref textBounds);

            _ascent = fontPaint.FontMetrics.Ascent;
            _descent = fontPaint.FontMetrics.Descent;
            _leading = fontPaint.FontMetrics.Leading;

            textBounds.Bottom = fontPaint.FontMetrics.Bottom;

            var textSize = new SKSize(textBounds.Size.Width, textBounds.Size.Height);
            var width = textSize.Width + PaddingLeft + PaddingRight;
            var height = textSize.Height + PaddingTop + PaddingBottom;
            Size = new SKSize(width, height);
        }
    }

    public string GetText()
    {
        return _mText;
    }

    public void Move(float x, float y)
    {
        Position = new SKPoint(x, y);
    }

    public void Draw(SKCanvas canvas)
    {
        DrawHeaderLine(canvas);
        DrawText(canvas);
    }

    private void DrawHeaderLine(SKCanvas canvas)
    {
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;
            paint.Color = ControlColors.DetailLineColors;
            paint.StrokeCap = SKStrokeCap.Round;
            paint.StrokeWidth = 1f;
            paint.Style = SKPaintStyle.StrokeAndFill;

            var smallsizeX = Size.Width;
            var smallsizeY = Size.Height;


            //var smallrect = new SKRect(Position.X, Position.Y, smallsizeX + Position.X, smallsizeY + Position.Y);
            //var smallroundRect = new SKRoundRect(smallrect, 5.0f);
            //canvas.DrawRect(smallrect, paint);
            canvas.DrawLine(new SKPoint(Position.X, smallsizeY + Position.Y),
                new SKPoint(smallsizeX + Position.X, smallsizeY + Position.Y), paint);
        }
    }

    private void DrawText(SKCanvas canvas)
    {
        var y = Position.Y + Size.Height - (PaddingBottom + _descent);
        var x = Position.X + PaddingLeft;
        using (var fontPaint = new SKPaint())
        {
            fontPaint.TextSize = 12.0f;
            fontPaint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright);
            fontPaint.IsAntialias = true;
            fontPaint.Color = ControlColors.EventFontColor;
            fontPaint.IsStroke = false;
            fontPaint.TextAlign = SKTextAlign.Left;
            canvas.DrawText(_mText, x, y, fontPaint);
        }
    }
}