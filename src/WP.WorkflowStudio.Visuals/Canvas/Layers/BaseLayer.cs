namespace WP.WorkflowStudio.Visuals.Canvas.Layers;

public abstract class BaseLayer
{
    public Rect Bounds { get; set; }
    public abstract void Render(SKCanvas canvas);
}