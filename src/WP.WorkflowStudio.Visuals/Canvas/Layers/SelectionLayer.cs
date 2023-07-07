using Avalonia.Input;

namespace WP.WorkflowStudio.Visuals.Canvas.Layers;

public class SelectionLayer : BaseLayer
{
    private readonly CanvasModel _canvasModel;
    private readonly Cursor _resizeCursor = new(StandardCursorType.SizeAll);

    public SelectionLayer(CanvasModel canvasModel, Rect bounds)
    {
        Bounds = bounds;
        _canvasModel = canvasModel;
    }

    public bool CanResize { get; private set; }

    private SKPoint CurrentPoint { get; set; } = SKPoint.Empty;

    public override void Render(SKCanvas canvas)
    {
        DrawSelectedElementBorder(canvas);
    }

    private void DrawSelectedElementBorder(SKCanvas canvas)
    {
        if (_canvasModel.SelectedElement != null)
        {
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.Blue,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 2.0f,
                IsStroke = true
            };

            var selectionBounds = _canvasModel.SelectedElement.GetRect();
            canvas.DrawRect(selectionBounds, paint);
            paint.IsStroke = false;
            var upperLeft = new SKPoint(selectionBounds.Left, selectionBounds.Top);
            var upperRight = new SKPoint(selectionBounds.Right, selectionBounds.Top);
            var lowerLeft = new SKPoint(selectionBounds.Left, selectionBounds.Bottom);
            var lowerRight = new SKPoint(selectionBounds.Right, selectionBounds.Bottom);

            canvas.DrawCircle(upperLeft, 2.0f, paint);
            canvas.DrawCircle(upperRight, 2.0f, paint);
            canvas.DrawCircle(lowerLeft, 2.0f, paint);
            canvas.DrawCircle(lowerRight, 2.0f, paint);

            var lr = new SKRect(lowerRight.X, lowerRight.Y, lowerRight.X + 4, lowerRight.Y + 4);

            var cursorRect = new SKRect(CurrentPoint.X, CurrentPoint.Y, CurrentPoint.X + 2, CurrentPoint.Y + 2);
            if (lr.IntersectsWith(cursorRect))
            {
                _canvasModel.CurrentCursor = _resizeCursor;
                CanResize = true;
            }
            else
            {
                _canvasModel.CurrentCursor = Cursor.Default;
                CanResize = false;
            }

            DrawEnlargeIcon(canvas);
        }
    }

    private void DrawEnlargeIcon(SKCanvas canvas)
    {
        if (_canvasModel.SelectedElement == null) return;

        using var paint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Blue,
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 4.0f,
            IsStroke = false
        };
        var selectionBounds = _canvasModel.SelectedElement.GetRect();
        var lowerRight = new SKPoint(selectionBounds.Right, selectionBounds.Bottom);

        SKPoint next;
        for (var i = 1; i <= 3; i++)
        {
            next = new SKPoint(lowerRight.X - 4, lowerRight.Y - i * 4);
            canvas.DrawCircle(next, 1.0f, paint);
        }

        for (var i = 1; i <= 2; i++)
        {
            next = new SKPoint(lowerRight.X - 8, lowerRight.Y - i * 4);
            canvas.DrawCircle(next, 1.0f, paint);
        }

        next = new SKPoint(lowerRight.X - 12, lowerRight.Y - 4);
        canvas.DrawCircle(next, 1.0f, paint);
    }

    public void SetPosition(SKPoint pos)
    {
        CurrentPoint = pos;
    }
}