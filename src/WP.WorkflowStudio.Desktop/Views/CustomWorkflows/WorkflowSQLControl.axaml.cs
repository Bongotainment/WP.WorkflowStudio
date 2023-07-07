using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaEdit;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.TextMate;
using SkiaSharp;
using TextMateSharp.Grammars;
using WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;
using SKSvg = Svg.Skia.SKSvg;

namespace WP.WorkflowStudio.Desktop.Views.CustomWorkflows;

public partial class WorkflowSqlControl : UserControl
{
    private CompletionWindow? _completionWindow;
    private OverloadInsightWindow? _insightWindow;

    public WorkflowSqlControl()
    {
        InitializeComponent();
        var sqlEditor = this.FindControl<TextEditor>("sqleditor");

        var _registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        if (sqlEditor != null)
        {
            var textMateInstallation = sqlEditor.InstallTextMate(_registryOptions);
            textMateInstallation.SetGrammar(
                _registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".SQL").Id));


            sqlEditor.TextArea.TextEntered += TextArea_TextEntered;
            sqlEditor.TextArea.TextEntering += TextArea_TextEntering;
        }
    }


    private void TextArea_TextEntering(object? sender, TextInputEventArgs e)
    {
        if (e.Text != null && e.Text.Length > 0 && _completionWindow != null)
            if (!char.IsLetterOrDigit(e.Text[0]))
                // Whenever a non-letter is typed while the completion window is open,
                // insert the currently selected element.
                _completionWindow.CompletionList.RequestInsertion(e);

        _insightWindow?.Hide();
    }

    private void TextArea_TextEntered(object? sender, TextInputEventArgs e)
    {
        // if (e.Text == " ")
        // {
        //     
        //
        //     _completionWindow = new CompletionWindow(_sqlEditor.TextArea);
        //     _completionWindow.Closed += (o, args) => _completionWindow = null;
        //
        //     _completionWindow.Width = 600;
        //     _completionWindow.CloseAutomatically = false;
        //
        //     var data = _completionWindow.CompletionList.CompletionData;
        //     var tables = ((WorkflowSQLViewModel)this.DataContext).GetTables();
        //
        //     Bitmap tableBitmap = GetTableBitmap();
        //     Bitmap columnBitmap = GetColumnBitmap();
        //
        //     foreach (var item in tables)
        //     {
        //         var completion = new SQLCompletionData(item.Schema_Name + "." + item.Object_Name);
        //         completion.Description = item.Description;
        //         if (tableBitmap != null) 
        //         {
        //             completion.Image = tableBitmap;
        //
        //         }
        //         data.Add(completion);
        //
        //         _completionWindow.Show();
        //     }
        // }
        // else if (e.Text == "(")
        // {
        //     _insightWindow = new OverloadInsightWindow(_sqlEditor.TextArea);
        //     _insightWindow.Closed += (o, args) => _insightWindow = null;
        //
        //     _insightWindow.Provider = new SQLOverloadProvider(new[]
        //     {
        //         ("Method1(int, string)", "Method1 description"),
        //         ("Method2(int)", "Method2 description"),
        //         ("Method3(string)", "Method3 description"),
        //     });
        //
        //     _insightWindow.Show();
        //}
    }

    private Bitmap? GetColumnBitmap()
    {
        return GetBitmap(new Uri("avares://WP.WorkflowStudio.Desktop/Assets/images/tableColumn.svg"));
    }

    private static Bitmap? GetTableBitmap()
    {
        return GetBitmap(new Uri("avares://WP.WorkflowStudio.Desktop/Assets/images/table.svg"));
    }

    private static Bitmap? GetBitmap(Uri uri)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        Bitmap? bitmap = null;
        if (assets == null) return bitmap;

        using (var svg = new SKSvg())
        {
            using (var stream = AssetLoader.Open(uri))
                // using (var stream = assets.Open(uri))
            {
                svg.Load(stream);
                using (var picture = svg.Picture)
                {
                    var settings = new SKSizeI(18, 18);
                    using (var skImage = SKImage.FromPicture(picture, settings))
                    {
                        var encoded = skImage.Encode(SKEncodedImageFormat.Png, 100);
                        using (var ms = new MemoryStream(encoded.ToArray()))
                        {
                            bitmap = new Bitmap(ms);
                        }
                    }
                }
            }
        }

        return bitmap;
    }

    private void ProceduresTreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems[0] is BaseTreeViewElement treeviewElement && DataContext is WorkflowSqlViewModel viewModel)
            try
            {
                viewModel.SelectedItem = treeviewElement;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
    }
}