using WP.WorkflowStudio.Visuals.Canvas.Layers.EventFlowElements;

namespace WP.WorkflowStudio.Visuals.Canvas.Layers;

public class EventFlowLayer : BaseLayer
{
    private readonly float _attraction; // Der Faktor zur Steuerung der Anziehungskraft zwischen den Rechtecken
    private readonly CanvasModel _canvasModel;
    private readonly float _repulsion; // Der Faktor zur Steuerung der Abstoßungskraft zwischen den Rechtecken


    public EventFlowLayer(CanvasModel canvasModel, Rect bounds)
    {
        Bounds = bounds;
        _canvasModel = canvasModel;
        _attraction = 0.5f;
        _repulsion = 0.5f;
    }

    public void Run(int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            var attractions = new Dictionary<EventView, SKPoint>();
            var repulsions = new Dictionary<EventView, SKPoint>();
            // Kräfte zwischen den Rechtecken berechnen
            var squares = _canvasModel.GetEventViews();

            foreach (var rectangle1 in squares)
            {
                var force = new SKPoint();
                foreach (var rectangle2 in squares)
                {
                    if (rectangle1 == rectangle2)
                        continue;

                    // Repulsion berechnen
                    var delta = SKPoint.Subtract(GetMidPoint(rectangle2), GetMidPoint(rectangle1));
                    var distance = SKPoint.Distance(GetMidPoint(rectangle2), GetMidPoint(rectangle1));
                    var direction = new SKPoint(delta.X / distance, delta.Y / distance);

                    if (distance >= 400) continue;
                    force -= new SKPoint(direction.X / _attraction, direction.Y / _attraction);
                }

                attractions[rectangle1] = force;
            }

            foreach (var calculated in attractions)
                calculated.Key.SetPosition(calculated.Key.Position += calculated.Value);

            foreach (var rectangle1 in squares)
            {
                var force = new SKPoint();
                foreach (var rectangle2 in squares)
                {
                    if (rectangle1 == rectangle2)
                        continue;

                    // Attraction berechnen

                    var delta = SKPoint.Subtract(GetMidPoint(rectangle2), GetMidPoint(rectangle1));
                    var distance = SKPoint.Distance(GetMidPoint(rectangle2), GetMidPoint(rectangle1));
                    var direction = new SKPoint(delta.X / distance, delta.Y / distance);

                    if (rectangle1.HasConnectionToEvent(rectangle2) || rectangle2.HasConnectionToEvent(rectangle1))
                    {
                        if (GetRectWithBorder(rectangle1.GetRect())
                            .IntersectsWith(GetRectWithBorder(rectangle2.GetRect())))
                            force -= new SKPoint(direction.X * _repulsion, direction.Y * _repulsion);
                        else if (distance > 400)
                            force += new SKPoint(direction.X / _attraction, direction.Y / _attraction);
                    }
                }

                repulsions[rectangle1] = force;
            }

            foreach (var calculated in repulsions)
                calculated.Key.SetPosition(calculated.Key.Position += calculated.Value);
        }
    }

    public SKRect GetRectWithBorder(SKRect rectangle1)
    {
        return SKRect.Inflate(rectangle1, 10, 10);
    }

    private SKPoint GetMidPoint(EventView eventView)
    {
        var rect = eventView.GetRect();
        return new SKPoint(rect.MidX, rect.MidY);
    }

    public override void Render(SKCanvas canvas)
    {
        DrawOnCanvas(canvas);
    }

    private void DrawOnCanvas(SKCanvas canvas)
    {
        var squares = _canvasModel.GetEventViews();
        try
        {
            foreach (var item in squares) item.Draw(canvas);

            foreach (var square in squares)
            {
                var children = square.GetChildren();
                foreach (var child in children)
                {
                    var childChildren = child.GetChildren();
                    foreach (var childEvent in childChildren) DrawConnection(canvas, child, childEvent);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void DrawConnection(SKCanvas canvas, BaseViewElement one, BaseViewElement two)
    {
        using (var linePaint = new SKPaint())
        {
            var colors = ControlColors.LineColors;

            linePaint.IsAntialias = true;
            linePaint.Color = SKColors.Black;
            linePaint.StrokeCap = SKStrokeCap.Round;
            linePaint.IsStroke = true;

            SKPoint p1;
            SKPoint middle1;
            SKPoint p;
            if (one.DockingPointRight().X - one.Size.Width / 2 < two.Position.X)
            {
                p = one.DockingPointRight();
                p1 = new SKPoint(p.X, p.Y);
                middle1 = new SKPoint(p1.X + 50, p1.Y);
            }
            else
            {
                p = one.DockingPointLeft();
                p1 = new SKPoint(p.X, p.Y);
                middle1 = new SKPoint(p1.X - 50, p1.Y);
            }

            var drawCubic = true;
            if (two.DockingPointBottom().Y < one.DockingPointTop().Y)
            {
                p = two.DockingPointBottom();
                drawCubic = false;
            }
            else
            {
                p = two.DockingPointTop();
            }

            if (one.DockingPointTop().Y - two.Size.Height < two.DockingPointTop().Y &&
                one.DockingPointBottom().Y + two.Size.Height > two.DockingPointBottom().Y)
            {
                if (one.DockingPointLeft().X > two.DockingPointRight().X)
                    p = two.DockingPointRight();
                else
                    p = two.DockingPointLeft();


                drawCubic = false;
            }


            var p2 = new SKPoint(p.X, p.Y);
            var middle2 = new SKPoint(p2.X, p2.Y - 50);

            // Draw path with quadratic Bezier
            using (var path = new SKPath())
            {
                path.MoveTo(p1);
                if (drawCubic)
                    path.CubicTo(middle1, middle2, p2);
                else
                    path.QuadTo(middle1, p2);


                var shader = SKShader.CreateLinearGradient(
                    p1,
                    p2,
                    colors,
                    null,
                    SKShaderTileMode.Clamp);
                linePaint.Shader = shader;

                canvas.DrawPath(path, linePaint);
            }

            DrawEndPoints(canvas, p1, p2);
        }
    }

    private static void DrawEndPoints(SKCanvas canvas, SKPoint p1, SKPoint p2)
    {
        using (var linePaint = new SKPaint())
        {
            linePaint.IsAntialias = true;
            linePaint.Color = ControlColors.LineDotColorStart;
            linePaint.StrokeCap = SKStrokeCap.Round;
            linePaint.IsStroke = false;

            linePaint.Color = ControlColors.LineDotColorStart;
            canvas.DrawCircle(p1, 4.0f, linePaint);

            linePaint.Color = ControlColors.LineDotColorEnd;
            canvas.DrawCircle(p2, 4.0f, linePaint);
        }
    }
}