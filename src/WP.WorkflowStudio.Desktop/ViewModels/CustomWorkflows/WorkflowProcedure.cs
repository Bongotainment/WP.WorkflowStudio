using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;

public partial class WorkflowProcedure : BaseTreeViewElement
{
    private readonly string _checkAction;

    private readonly string _createProcedure;
    private readonly string _procedureBody;
    private readonly string _setDisplayNameProcedure;

    private string _displayName = string.Empty;
    private string _name = string.Empty;
    private string _sqlText = "";

    [ObservableProperty] private ObservableCollection<WorkflowParameter> _workflowParameters;

    public WorkflowProcedure()
    {
        _workflowParameters = new ObservableCollection<WorkflowParameter>();
        _workflowParameters.CollectionChanged += WorkflowParametersOnCollectionChanged;
        _checkAction = @"EXEC CustomWorkflows._CheckAction @actionName ='__ACTIONNAME__'";
        _setDisplayNameProcedure =
            @"EXEC CustomWorkflows._SetActionDisplayName @actionName ='__ACTIONNAME__', @displayName ='__DISPLAYNAME__'";
        _createProcedure =
            @"CREATE PROCEDURE CustomWorkflows.__ACTIONNAME__ __STANDARDPARAMETER__ __STANDARDPARAMETERTYPE__";
        var builder = new StringBuilder();
        builder.AppendLine("AS");
        builder.AppendLine("BEGIN");
        builder.AppendLine("    --ENTER CODE HERE");
        builder.AppendLine("END");
        builder.AppendLine("GO");
        _procedureBody = builder.ToString();
    }


    [Required(ErrorMessage = "Das Feld ist erforderlich")]
    [MaxLength(100, ErrorMessage = "Das Feld darf maximal 100 Zeichen enthalten.")]
    [RegularExpression("([A-Za-z_][A-Za-z0-9_]*)",
        ErrorMessage =
            "Das Feld darf nur Buchstaben, Zahlen und Unterstriche enthalten. Es muss mit einem Buchstaben beginnen")]
    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value, true);
            CalcSQLText();
            OnObjectChanged();
        }
    }

    [Required(ErrorMessage = "Das Feld ist erforderlich")]
    [MaxLength(100, ErrorMessage = "Das Feld darf maximal 100 Zeichen enthalten.")]
    public string DisplayName
    {
        get => _displayName;
        set
        {
            SetProperty(ref _displayName, value, true);
            CalcSQLText();
            OnPropertyChanged(nameof(TreeViewDisplayName));
            OnObjectChanged();
        }
    }

    public override string TreeViewDisplayName => DisplayName;

    public string SQLText
    {
        set
        {
            SetProperty(ref _sqlText, value, true);
            OnObjectChanged();
        }
        get => _sqlText;
    }

    public event EventHandler? OnRequestClose;
    public event EventHandler? OnRequestExit;

    private void WorkflowParametersOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CalcSQLText();
    }

    public void AddOrUpdateParameter(WorkflowParameter inputParameter)
    {
        var workflowParameter = GetWorkflowParameterByIdentity(inputParameter);
        if (workflowParameter != null)
        {
            var index = WorkflowParameters.IndexOf(workflowParameter);
            if (index >= 0) WorkflowParameters[index] = workflowParameter.ShallowCopy();
        }
        else
        {
            WorkflowParameters.Add(inputParameter.ShallowCopy());
        }

        OnObjectChanged();
    }

    public void RemoveParameter(WorkflowParameter inputParameter)
    {
        if (inputParameter == null || inputParameter is not WorkflowParameter parameter) return;

        var workflowParameter = GetWorkflowParameterByIdentity(parameter);
        if (workflowParameter == null) return;
        WorkflowParameters.Remove(workflowParameter);
    }

    private WorkflowParameter? GetWorkflowParameterByIdentity(WorkflowParameter selectedParameter)
    {
        WorkflowParameter? workflowParameter = null;
        if (WorkflowParameters.Any(x => x.Identity == selectedParameter.Identity))
            workflowParameter = WorkflowParameters
                .First(x => x.Identity == selectedParameter.Identity);

        return workflowParameter;
    }

    private void CalcSQLText()
    {
        var parameterSQL = new StringBuilder();
        foreach (var param in WorkflowParameters) parameterSQL.AppendLine(param.SQLText);
        var builder = new StringBuilder();
        builder.AppendLine(parameterSQL.ToString());

        var newCreateType = string.Empty;
        var newExecType = string.Empty;

        var newCreateProcedure = _createProcedure.Replace("__STANDARDPARAMETER__", "@kArtikel");
        newCreateProcedure = newCreateProcedure.Replace("__STANDARDPARAMETERTYPE__", "int");
        newCreateProcedure = newCreateProcedure.Replace("__ACTIONNAME__", Name);
        foreach (var param in WorkflowParameters)
            newCreateProcedure += $", {param.ParameterActionName} {param.AllowedDatatype.SqlTypeName}";
        builder.AppendLine(newCreateProcedure);
        builder.AppendLine(_procedureBody);
        var newDisplayNameProcedure = _setDisplayNameProcedure
            .Replace("__ACTIONNAME__", Name)
            .Replace("__DISPLAYNAME__", DisplayName);

        builder.AppendLine(newDisplayNameProcedure);
        var newCheckAction = _checkAction.Replace("__ACTIONNAME__", Name);
        builder.AppendLine(newCheckAction);
        SQLText = builder.ToString();
    }

    public WorkflowProcedure ShallowCopy()
    {
        return new WorkflowProcedure
        {
            Name = _name,
            DisplayName = _displayName,
            WorkflowParameters = WorkflowParameters,
            Identity = Identity
        };
    }

    [RelayCommand]
    public void CloseClicked()
    {
        if (OnRequestClose == null) return;

        OnRequestClose(this, new EventArgs());
    }

    [RelayCommand]
    public void SaveClicked()
    {
        if (OnRequestExit == null) return;

        OnRequestExit(this, EventArgs.Empty);
    }

    public bool ContainsParameter(WorkflowParameter parameter)
    {
        return WorkflowParameters.Contains(parameter);
    }
}