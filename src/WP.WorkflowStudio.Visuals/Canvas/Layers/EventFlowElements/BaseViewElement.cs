namespace WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

public class BaseViewElement
{
    public SKSize Size { get; set; }
    public float MarginTop { get; set; }
    public float MarginLeft { get; set; }
    public float MarginRight { get; set; }
    public float MarginBottom { get; set; }
    public SKPoint Position { get; set; }

    public float PaddingTop { get; set; }
    public float PaddingLeft { get; set; }
    public float PaddingRight { get; set; }
    public float PaddingBottom { get; set; }

    public SKPoint DockingPointLeft()
    {
        return new SKPoint(Position.X, Position.Y + Size.Height / 2);
    }

    public SKPoint DockingPointRight()
    {
        return new SKPoint(Position.X + Size.Width, Position.Y + Size.Height / 2);
    }

    public SKPoint DockingPointTop()
    {
        return new SKPoint(Position.X + (Size.Width + 20) / 2, Position.Y);
    }

    public SKPoint DockingPointBottom()
    {
        return new SKPoint(Position.X + (Size.Width + 20) / 2, Position.Y + Size.Height);
    }

    public SKSize GetOuterSize()
    {
        return new SKSize(Size.Height + MarginBottom + MarginTop, Size.Width + MarginLeft + MarginRight);
    }

    public SKRect GetRect()
    {
        var left = Position.X;
        var top = Position.Y;
        var right = Position.X + Size.Width;
        var bottom = Position.Y + Size.Height;
        return new SKRect(left, top, right, bottom);
    }
}