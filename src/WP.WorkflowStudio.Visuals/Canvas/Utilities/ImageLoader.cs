namespace WP.WorkflowStudio.Visuals.Canvas.Utilities;

public class ImageLoader
{
    private readonly Dictionary<string, SKSvg> loadedImages = new();

    internal SKSvg GetImageByName(string imageName)
    {
        SKSvg? svg;
        if (loadedImages.TryGetValue(imageName, out svg)) return svg;
        svg = new SKSvg();
        Console.WriteLine(imageName);
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        if (assets == null) return svg;
        var path = Path.Combine("avares://WP.WorkflowStudio.Visuals/Assets/Images", imageName);
        var uri = new Uri(path);
        var a = assets.Exists(uri);
        if (a)
        {
            var b = assets.Open(uri);
            svg.Load(b);
        }

        loadedImages.Add(imageName, svg);
        return svg;
    }
}