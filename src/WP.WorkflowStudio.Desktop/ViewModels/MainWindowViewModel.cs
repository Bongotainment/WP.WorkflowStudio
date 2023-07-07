using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WP.WorkflowStudio.Core.Models;
using WP.WorkflowStudio.Desktop.Services;
using WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;
using WP.WorkflowStudio.Desktop.Views.Menu;
using WP.WorkflowStudio.Repositories;
using WP.WorkflowStudio.Visuals.Canvas;
using Color = System.Drawing.Color;

namespace WP.WorkflowStudio.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly WorkflowColorService _colorService;
    private readonly DialogService _dialogService;
    private readonly LoginWindowViewModel _loginWindowViewModel;
    private readonly WorkflowRepository _workflowRepository;

    [ObservableProperty] private Dictionary<int, IBrush> _categoryIdColors;

    [ObservableProperty] private IBrush _ClickedColor;

    [ObservableProperty] private WorkflowPropertyGridElement? _clickedWorkflowPropertyGridElement;

    [ObservableProperty] private bool _isPanelClosed;

    [ObservableProperty] private Dictionary<string, IBrush> _namedCategoryColors;

    [ObservableProperty] private int _openPaneLength = 300;

    [ObservableProperty] private string _splitViewImagePath = "/Assets/images/collapse.svg";

    [ObservableProperty] private ObservableCollection<WorkflowEvent> _workflowEvents;

    [ObservableProperty] private WorkflowSqlViewModel _workflowSqlViewModel;

#pragma warning disable CS8618
    public MainWindowViewModel()
    {
        _namedCategoryColors = new Dictionary<string, IBrush>();
        _categoryIdColors = new Dictionary<int, IBrush>();
    }
#pragma warning restore CS8618

    public MainWindowViewModel(WorkflowRepository workflowRepository, LoginWindowViewModel loginWindowViewModel,
        DialogService dialogService, WorkflowColorService colorService, WorkflowSqlViewModel workflowSqlViewModel)
    {
        _workflowRepository = workflowRepository;
        _loginWindowViewModel = loginWindowViewModel;
        _dialogService = dialogService;
        _colorService = colorService;
        _workflowSqlViewModel = workflowSqlViewModel;
        _ClickedColor = Brushes.Black;
        _workflowEvents = new ObservableCollection<WorkflowEvent>();
        _namedCategoryColors = _colorService.GetNamedCategoryColors();
        _categoryIdColors = _colorService.GetCategoryIdColors();
    }

    internal LoginWindowViewModel GetLoginWindowViewModel()
    {
        return _loginWindowViewModel;
    }

    private void SetWorkflowColors(IEnumerable<WorkflowEvent> workflowEvents)
    {
        var brush = ClickedColor;
        foreach (var workflow in workflowEvents)
        {
            brush = CategoryIdColors[workflow.ObjectId] as ISolidColorBrush;
            var avaloniaBrush = (ISolidColorBrush)CategoryIdColors[workflow.ObjectId];
            var avaloniaColor = avaloniaBrush.Color;
            var color = Color.FromArgb(avaloniaColor.A, avaloniaColor.R, avaloniaColor.G, avaloniaColor.B);
            workflow.Color = color;
        }

        ClickedColor = brush;
    }

    [RelayCommand]
    public void AllWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var empty = Array.Empty<int>();
        var workflows = _workflowRepository.GetWorkflows(empty);
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(workflows);
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void ProcurementWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetProcurementWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void ArticleWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetArticleWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void CustomerWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetCustomersWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void OrderWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetOrderWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void DeliveryWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetDeliveryWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void InvoiceWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetInvoiceWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void PicklistWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetPicklistWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void OfferWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetOfferWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void ServicedeskWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetServicedeskWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void ReturnsWorkflowsClicked(MenuButtonClickedEventArgs args)
    {
        var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetReturnsWorkflows());
        SetWorkflowColors(WorkflowEvents);
        this.WorkflowEvents = WorkflowEvents;
    }

    [RelayCommand]
    public void ToggleWorkflowProperties()
    {
        IsPanelClosed = !IsPanelClosed;
        if (IsPanelClosed)
        {
            SplitViewImagePath = "/Assets/images/expand.svg";
            OpenPaneLength = 50;
        }
        else
        {
            SplitViewImagePath = "/Assets/images/collapse.svg";
            OpenPaneLength = 300;
        }
    }

    [RelayCommand]
    public void ElementClicked(ElementClickedEventArgs args)
    {
        ClickedWorkflowPropertyGridElement = new WorkflowPropertyGridElement
        {
            ClickedFlow = args.FlowName,
            ClickedFlowEventName = args.EventName,
            ClickedFlowAvarageRuntime = "Not Implemented",
            IsCustomWorkflow = args.IsCustomWorkflow,
            WasExecutedInPast = args.WasExecutedInPast,
            HasCustomCondition = args.HasCustomCondition,
            ExecuteDelayed = args.ExecuteDelayed,
            Activated = args.Activated
        };
    }

    public async Task ShowLoginDialog()
    {
        if (_loginWindowViewModel == null) return;
        var isLoggedIn = _loginWindowViewModel.IsDBAuthorized;
        if (!isLoggedIn) isLoggedIn = await _dialogService.ShowLoginDialog(_loginWindowViewModel);
        if (isLoggedIn && _loginWindowViewModel.IsAuthorized)
        {
            var WorkflowEvents = new ObservableCollection<WorkflowEvent>(_workflowRepository.GetWorkflows(6));
            SetWorkflowColors(WorkflowEvents);
            this.WorkflowEvents = WorkflowEvents;
        }
    }
}