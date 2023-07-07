using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.Input;
using WP.WorkflowStudio.Desktop.Services;

namespace WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;

public partial class WorkflowSqlViewModel : ViewModelBase
{
    private readonly DialogService? _dialogService;
    private readonly WawiDbDocumentationService? _documentationService;

    private WorkflowParameter? _currentWorkflowParameter;

    private WorkflowProcedure? _currentWorkflowProcedure;

    private BaseTreeViewElement? _selectedItem;


    private string _sqlText = "";

    public WorkflowSqlViewModel()
    {
        WorkflowProcedures = new ObservableCollection<WorkflowProcedure>();
        OnPropertyChanged(nameof(WorkflowProcedures));
    }

    public WorkflowSqlViewModel(WawiDbDocumentationService documentationService, DialogService dialogService) : this()
    {
        _dialogService = dialogService;
        _documentationService = documentationService;
    }

    public BaseTreeViewElement? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            if (_selectedItem is WorkflowProcedure procedure)
                CurrentWorkflowProcedure = procedure;
            else if (_selectedItem is WorkflowParameter parameter) CurrentWorkflowParameter = parameter;

            OnPropertyChanged(nameof(IsParameterSelected));
            OnPropertyChanged(nameof(IsProcedureSelected));
        }
    }

    public WorkflowParameter? CurrentWorkflowParameter
    {
        get => _currentWorkflowParameter;
        set
        {
            if (_currentWorkflowParameter != null)
                _currentWorkflowParameter.ObjectChanged -= CurrentWorkflowParameterOnObjectChanged;

            SetProperty(ref _currentWorkflowParameter, value);
            RecalcSqlText();
            SaveCompleteCommand.NotifyCanExecuteChanged();
            if (_currentWorkflowParameter != null)
                _currentWorkflowParameter.ObjectChanged += CurrentWorkflowParameterOnObjectChanged;
        }
    }

    public WorkflowProcedure? CurrentWorkflowProcedure
    {
        get => _currentWorkflowProcedure;
        set
        {
            if (_currentWorkflowProcedure != null)
                _currentWorkflowProcedure.ObjectChanged -= CurrentWorkflowProcedureOnObjectChanged;
            SetProperty(ref _currentWorkflowProcedure, value);
            RecalcSqlText();
            SaveCompleteCommand.NotifyCanExecuteChanged();
            if (_currentWorkflowProcedure != null)
                _currentWorkflowProcedure.ObjectChanged += CurrentWorkflowProcedureOnObjectChanged;
        }
    }

    public string SqlText
    {
        get => _sqlText;
        set => SetProperty(ref _sqlText, value);
    }

    public ObservableCollection<WorkflowProcedure> WorkflowProcedures { get; set; }

    private void CurrentWorkflowProcedureOnObjectChanged(object? sender, EventArgs e)
    {
        SaveCompleteCommand.NotifyCanExecuteChanged();
    }

    private void CurrentWorkflowParameterOnObjectChanged(object? sender, EventArgs e)
    {
        SaveCompleteCommand.NotifyCanExecuteChanged();
    }

    private void RecalcSqlText()
    {
        if (CurrentWorkflowProcedure == null) return;

        var procedureSql = new StringBuilder();
        procedureSql.AppendLine(CurrentWorkflowProcedure.SQLText);
        SqlText = procedureSql.ToString();
    }

    #region -= TreeView Handling =-

    [RelayCommand]
    public Task ItemDoubleTapped(object tappedItem)
    {
        if (tappedItem is WorkflowProcedure workflowProcedure)
        {
            var selected = new List<WorkflowProcedure> { workflowProcedure };
            SelectionModelSelectionChangedEventArgs args =
                new SelectionModelSelectionChangedEventArgs<WorkflowProcedure>(selectedItems: selected);
        }
        else if (tappedItem is WorkflowParameter workflowParameter)
        {
            var selected = new List<WorkflowParameter> { workflowParameter };
            SelectionModelSelectionChangedEventArgs args =
                new SelectionModelSelectionChangedEventArgs<WorkflowParameter>(selectedItems: selected);
        }

        return Task.CompletedTask;
    }

    #endregion

    public List<DocumentationEntry> GetTables()
    {
        var returnValue = _documentationService?.Root?.Tables;
        if (returnValue == null) returnValue = new List<DocumentationEntry>();

        return returnValue;
    }


    #region -= Remove =-

    private async Task Remove(object? removedItem)
    {
        if (removedItem is WorkflowProcedure workflowProcedure)
            await RemoveProcedure(workflowProcedure);
        else if (removedItem is WorkflowParameter workflowParameter)
            await RemoveParameterFromProcedure(workflowParameter);
    }

    private Task RemoveProcedure(WorkflowProcedure selectedProcedure)
    {
        WorkflowProcedures.Remove(selectedProcedure);
        return Task.CompletedTask;
    }

    private async Task AddParameterToProcedure(WorkflowProcedure procedure)
    {
        try
        {
            if (_dialogService == null) return;
            var parameter = new WorkflowParameter();
            if (await _dialogService.ShowParameterDialog(parameter))
            {
                CurrentWorkflowParameter = parameter;
                procedure.AddOrUpdateParameter(CurrentWorkflowParameter);
                OnPropertyChanged(nameof(WorkflowProcedures));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async Task AddNewProcedure()
    {
        if (_dialogService == null) return;
        var procedure = new WorkflowProcedure();
        procedure.DisplayName = "Neu";
        procedure.Name = "Neu";
        if (await _dialogService.ShowProcedureDialog(procedure)) WorkflowProcedures.Add(procedure);
    }

    private Task RemoveParameterFromProcedure(WorkflowParameter parameter)
    {
        var procedure = WorkflowProcedures.First(procedure => procedure.ContainsParameter(parameter));
        procedure.RemoveParameter(parameter);
        return Task.CompletedTask;
    }

    #endregion

    #region -= Menu Handling =-

    [RelayCommand]
    public async Task Menu_AddParameter(object procedure)
    {
        if (procedure is WorkflowProcedure workflowProcedure) await AddParameterToProcedure(workflowProcedure);
    }

    [RelayCommand]
    public async Task Menu_AddProcedure()
    {
        await AddNewProcedure();
    }

    [RelayCommand]
    public async Task Menu_Remove(object selectedParameter)
    {
        await Remove(selectedParameter);
    }

    public bool IsParameterSelected => SelectedItem is WorkflowParameter;

    public bool IsProcedureSelected => SelectedItem is WorkflowProcedure;

    #endregion

    #region -= TreeViewButtons Handling =-

    [RelayCommand]
    public async Task AddButtonPressed(object? tappedItem)
    {
        if (tappedItem is WorkflowProcedure workflowProcedure)
        {
            await AddParameterToProcedure(workflowProcedure);
        }
        else if (tappedItem is WorkflowParameter)
        {
            await AddNewProcedure();
        }
        else if (tappedItem is null)
        {
            await AddNewProcedure();
        }
        else
        {
            Console.WriteLine("Adds new Procedure but the parameter was weird");
            await AddNewProcedure();
        }
    }

    [RelayCommand]
    public async Task RemoveButtonPressed(object? tappedItem)
    {
        await Remove(tappedItem);
    }

    #endregion

    #region -= SQL Editor Button Handling =-

    [RelayCommand]
    public Task CancelComplete()
    {
        return Task.CompletedTask;
        //todo
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    public Task SaveComplete()
    {
        return Task.CompletedTask;
        //Todo
    }

    public bool IsValid => true;

    #endregion
}