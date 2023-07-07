using Avalonia.Input;
using WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;
using WP.WorkflowStudio.Visuals.Canvas.Utilities;

namespace WP.WorkflowStudio.Visuals.Canvas;

public class CanvasModel
{
    internal SKMatrix _matrix;

    public CanvasModel()
    {
        ImageLoader = new ImageLoader();
        CurrentCursor = Cursor.Default;
        Squares = new List<EventView>();
        Zoom = 1f;
        DraggedElement = null;
        TranslationStartClick = new SKPoint(0, 0);
        Coordinates = new SKPoint(0, 0);
        _matrix = SKMatrix.CreateIdentity();
    }

    private List<EventView> Squares { get; }
    internal float Zoom { get; set; }

    internal float MaxZoom { get; set; } = 2f;
    internal float MinZoom { get; set; } = 0.25f;
    internal EventView? DraggedElement { get; set; }
    internal EventView? SelectedElement { get; set; }

    internal SKPoint TranslationStartClick { get; set; }
    internal SKPoint ResizeStartClick { get; set; } = new(0, 0);
    internal SKPoint Coordinates { get; set; }

    internal Cursor CurrentCursor { get; set; }

    internal ImageLoader ImageLoader { get; }

    internal void ResetEventViews()
    {
        Squares.Clear();
    }

    internal void AddEventView(EventView eventView)
    {
        Squares.Add(eventView);
    }

    internal List<EventView> GetEventViews()
    {
        return Squares.ToList();
    }
}