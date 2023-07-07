namespace WP.WorkflowStudio.Visuals.Canvas;

//Static class...its like 3.6 Roentgen... Not great not terrible
internal static class ControlColors
{
    //Dark
    //
    // public static SKColor CanvasBackground  { get; set; }= new SKColor(37, 37, 37);
    // public static SKColor CanvasGridColor { get; set; } = new SKColor(33, 45, 70);
    //
    // public static SKColor[] LineColors { get; set; } = {
    //             SKColors.Yellow,
    //             SKColors.Magenta
    //         };
    //
    // public static SKColor EventBackground { get; set; } = new SKColor(24, 24, 24);
    // public static SKColor EventFontColor { get; set; } = SKColors.White;
    // public static SKColor DetailLineColors { get; set; } = new SKColor(239, 237, 236);
    // public static SKColor ConnectionLineColor { get; set; } = new SKColor(134, 132, 130);
    // public static SKColor StandardFontColor { get; set; } = new SKColor(122, 122, 122);
    // public static SKColor ShadowColor { get; set; } = new SKColor(0, 0, 0).WithAlpha(0x24);
    //
    // public static SKColor LineDotColorStart { get; set; } = SKColors.Yellow;
    // public static SKColor LineDotColorEnd { get; set; } = SKColors.Magenta;
    //     
    // public static SKColor HasCustomConditionColor = SKColors.Indigo;
    // public static SKColor IsCustomWorkflowColor = SKColors.Orange;
    // public static SKColor WorkflowInactiveColor = SKColors.LightGray;
    // public static SKColor TextColor { get; set; }= SKColors.White;

    //Light

    public static SKColor CanvasBackground { get; set; } = new(225, 223, 221);
    public static SKColor CanvasGridColor { get; set; } = new(225, 223, 221);

    public static SKColor EventBackground { get; set; } = new(255, 255, 255);
    public static SKColor EventFontColor { get; set; } = new(122, 122, 122);
    public static SKColor DetailLineColors { get; set; } = new(239, 237, 236);
    public static SKColor ConnectionLineColor { get; set; } = new(134, 132, 130);
    public static SKColor StandardFontColor { get; set; } = new(122, 122, 122);
    public static SKColor ShadowColor { get; set; } = new SKColor(0, 0, 0).WithAlpha(0x24);

    public static SKColor[] LineColors { get; set; } =
    {
        new(134, 132, 130),
        new(134, 132, 130)
    };

    public static SKColor LineDotColorStart { get; set; } = SKColors.Yellow;
    public static SKColor LineDotColorEnd { get; set; } = SKColors.Magenta;

    public static SKColor HasCustomConditionColor { get; set; } = SKColors.Indigo;
    public static SKColor IsCustomWorkflowColor { get; set; } = SKColors.Orange;
    public static SKColor WorkflowInactiveColor { get; set; } = SKColors.LightGray;
    public static SKColor TextColor { get; set; } = SKColors.Black;
}