namespace WP.WorkflowStudio.Visuals.Canvas.Layers;

public class DebugInformationBaseLayer : BaseLayer
{
    private readonly CanvasModel _canvasModel;

    public DebugInformationBaseLayer(CanvasModel canvasModel, Rect bounds)
    {
        Bounds = bounds;
        _canvasModel = canvasModel;
    }

    private SKPoint CurrentPoint { get; set; } = SKPoint.Empty;

    public override void Render(SKCanvas canvas)
    {
        DrawZoomLevel(canvas);
        DrawMouseCoordinates(canvas, _canvasModel.Coordinates);
        DrawMousePointer(canvas);
    }

    public void SetPosition(SKPoint pos)
    {
        CurrentPoint = pos;
    }

    private void DrawMousePointer(SKCanvas canvas)
    {
        using var paint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Blue,
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 1.0f,
            IsStroke = true
        };
        canvas.DrawCircle(CurrentPoint, 2.0f, paint);
    }

    private void DrawZoomLevel(SKCanvas canvas)
    {
        using (var fontPaint = new SKPaint())
        {
            var textBounds = new SKRect();

            fontPaint.TextSize = 12.0f;
            fontPaint.IsAntialias = true;
            fontPaint.Color = ControlColors.StandardFontColor;
            fontPaint.IsStroke = false;
            fontPaint.TextAlign = SKTextAlign.Left;
            var zoomLevel = Convert.ToDecimal(_canvasModel.Zoom);
            var zoomText = string.Concat(zoomLevel.ToString("P"));
            fontPaint.MeasureText(zoomText, ref textBounds);

            var y = (float)Bounds.Size.Height; // - textBounds.Height);
            var x = (float)Bounds.Size.Width - textBounds.Width;

            canvas.DrawText(zoomText, x, y, fontPaint);
        }
    }

    private void DrawMouseCoordinates(SKCanvas canvas, SKPoint coordinates)
    {
        using (var fontPaint = new SKPaint())
        {
            var textBounds = new SKRect();
            fontPaint.TextSize = 12.0f;
            fontPaint.IsAntialias = true;
            fontPaint.Color = ControlColors.StandardFontColor;
            fontPaint.IsStroke = false;
            fontPaint.TextAlign = SKTextAlign.Left;
            var strCoordinate = string.Concat(coordinates.X, " : ", coordinates.Y);

            fontPaint.MeasureText(strCoordinate, ref textBounds);

            var y = (float)Bounds.Size.Height;
            var x = (float)Bounds.Size.Width - textBounds.Width - 100;
            canvas.DrawText(strCoordinate, x, y, fontPaint);

            strCoordinate = string.Concat(coordinates.X / _canvasModel.Zoom, " : ", coordinates.Y / _canvasModel.Zoom);
            fontPaint.MeasureText(strCoordinate, ref textBounds);
            y = (float)Bounds.Size.Height - textBounds.Height - 10;
            x = (float)Bounds.Size.Width - textBounds.Width - 100;
            canvas.DrawText(strCoordinate, x, y, fontPaint);

            var newPoints = _canvasModel._matrix.MapPoint(coordinates);
            strCoordinate = string.Concat(newPoints.X, " : ", newPoints.Y);
            fontPaint.MeasureText(strCoordinate, ref textBounds);
            y = (float)Bounds.Size.Height - textBounds.Height - 40;
            x = (float)Bounds.Size.Width - textBounds.Width - 100;
            canvas.DrawText(strCoordinate, x, y, fontPaint);
        }
    }
}