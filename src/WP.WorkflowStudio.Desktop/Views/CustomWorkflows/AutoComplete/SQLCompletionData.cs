using System;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;

namespace WP.WorkflowStudio.Desktop.Views.CustomWorkflows.AutoComplete;

public class SqlCompletionData : ICompletionData
{
    public SqlCompletionData(string text)
    {
        Text = text;
    }

    public string Text { get; }

    // Use this property if you want to show a fancy UIElement in the list.
    public object Content => Text;

    public object? Description { get; set; }

    public double Priority { get; } = 0;

    public IBitmap? Image { get; set; }

    public void Complete(TextArea textArea, ISegment completionSegment,
        EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment, Text);
    }
}