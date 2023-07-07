using System.Collections.Generic;
using System.ComponentModel;
using AvaloniaEdit.CodeCompletion;

namespace WP.WorkflowStudio.Desktop.Views.CustomWorkflows.AutoComplete;

public class SQLOverloadProvider : IOverloadProvider
{
    private readonly IList<(string header, string content)> _items;
    private int _selectedIndex;

    public SQLOverloadProvider(IList<(string header, string content)> items)
    {
        _items = items;
        SelectedIndex = 0;
        CurrentIndexText = string.Empty;
    }


    public int SelectedIndex { get; set; }

    public int Count => _items.Count;

    public string CurrentIndexText { get; }

    public object CurrentHeader => _items[SelectedIndex].header;

    public object CurrentContent => _items[SelectedIndex].content;

    public event PropertyChangedEventHandler? PropertyChanged;
}