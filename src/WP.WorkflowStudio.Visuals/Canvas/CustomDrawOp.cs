using Avalonia.Rendering.SceneGraph;
using WP.WorkflowStudio.Visuals.Canvas.Layers;

namespace WP.WorkflowStudio.Visuals.Canvas;

internal class CustomDrawOp : ICustomDrawOperation
{
    private readonly CanvasModel _canvasModel;
    private readonly DebugInformationBaseLayer _debugInformationBaseLayer;
    private readonly EventFlowLayer _eventFlowLayer;
    private readonly SelectionLayer _selectionLayer;

    public CustomDrawOp(Rect bounds, CanvasModel canvasModel, DebugInformationBaseLayer debugInformationBaseLayer
        , EventFlowLayer eventFlowLayer, SelectionLayer selectionLayer)
    {
        _canvasModel = canvasModel;
        _debugInformationBaseLayer = debugInformationBaseLayer;
        _eventFlowLayer = eventFlowLayer;
        _selectionLayer = selectionLayer;
        Bounds = bounds;
    }

    public void Dispose()
    {
        // No-op
    }

    public Rect Bounds { get; }

    public bool HitTest(Point p)
    {
        return false;
    }

    public bool Equals(ICustomDrawOperation? other)
    {
        return false;
    }

    public void Render(ImmediateDrawingContext context)
    {
        _debugInformationBaseLayer.Bounds = Bounds;
        _eventFlowLayer.Bounds = Bounds;
        _selectionLayer.Bounds = Bounds;


        var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature == null)
            return;
        using var lease = leaseFeature.Lease();
        var canvas = lease.SkCanvas;

        if (canvas != null)
        {
            canvas.Save();
            canvas.Concat(ref _canvasModel._matrix);

            canvas.Clear(ControlColors.CanvasBackground);

            DrawGrid(canvas);
            _eventFlowLayer.Render(canvas);
            _selectionLayer.Render(canvas);
            canvas.Restore();
#if DEBUG
            _debugInformationBaseLayer.Render(canvas);
#endif
        }
    }

    private void DrawGrid(SKCanvas canvas)
    {
        var paint = new SKPaint
        {
            IsAntialias = true,
            Color = ControlColors.CanvasGridColor,
            StrokeCap = SKStrokeCap.Round
        };

        var maxX = (float)Bounds.Size.Width + 100000 / _canvasModel.Zoom;
        var maxY = (float)Bounds.Size.Height + 100000 / _canvasModel.Zoom;

        float startX = -100000;
        float startY = -100000;


        for (var currentX = startX; currentX <= maxX; currentX += 20)
        {
            var start = new SKPoint(currentX, startY);
            var end = new SKPoint(currentX, maxY);
            canvas.DrawLine(start, end, paint);
        }

        for (var currentY = startY; currentY <= maxY; currentY += 20)
        {
            var start = new SKPoint(startX, currentY);
            var end = new SKPoint(maxX, currentY);
            canvas.DrawLine(start, end, paint);
        }
    }
}