using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WP.WorkflowStudio.Desktop.ViewModels;

public partial class WorkflowPropertyGridElement : ViewModelBase
{
    [ObservableProperty] private string _actions = string.Empty;

    [ObservableProperty] private bool _activated;

    [ObservableProperty] private string[] _applications = Array.Empty<string>();
    [ObservableProperty] private string _clickedFlow = string.Empty;

    [ObservableProperty] private string _clickedFlowAvarageRuntime = string.Empty;

    [ObservableProperty] private string _clickedFlowEventName = string.Empty;

    [ObservableProperty] private string _conditions = string.Empty;

    [ObservableProperty] private bool _executeDelayed;

    [ObservableProperty] private bool _hasCustomCondition;

    [ObservableProperty] private bool _isCustomWorkflow;

    [ObservableProperty] private bool _wasExecutedInPast;

    [ObservableProperty] private string _workflowType = string.Empty;
}