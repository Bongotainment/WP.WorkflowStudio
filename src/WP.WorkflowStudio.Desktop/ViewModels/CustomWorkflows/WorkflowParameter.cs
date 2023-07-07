using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;

public partial class WorkflowParameter : BaseTreeViewElement
{
    private readonly string createtype;
    private readonly string exectype;


    private AllowedDatatype _allowedDatatype = new();

    private List<AllowedDatatype> _allowedDatatypes = new();


    private string _displayName = string.Empty;

    private string _name = string.Empty;


    private string _parameterActionName = string.Empty;

    private string _sqlText = "";

    public WorkflowParameter()
    {
        Identity = Guid.NewGuid();
        createtype = @"CREATE TYPE CustomWorkflows.__CREATETYPE__ FROM __DATATYPE__";
        exectype =
            @"EXEC CustomWorkflows._SetActionParameterDisplayName @parameterName = '__CREATETYPE__', @displayName = '__TYPEDISPLAYNAME__'";
        InitializeAllowedDatatypes();

        Name = "ActionName";
        DisplayName = "Aktion";
        AllowedDatatype = AllowedDatatypes.First();
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

    [Required(ErrorMessage = "Das Feld ist erforderlich")]
    [MaxLength(100, ErrorMessage = "Das Feld darf maximal 100 Zeichen enthalten.")]
    public string ParameterActionName
    {
        get => _parameterActionName;
        set
        {
            SetProperty(ref _parameterActionName, value, true);
            CalcSQLText();
            OnObjectChanged();
        }
    }

    [Required(ErrorMessage = "Das Feld ist erforderlich")]
    public AllowedDatatype AllowedDatatype
    {
        get => _allowedDatatype;
        set
        {
            SetProperty(ref _allowedDatatype, value, true);
            CalcSQLText();
            OnPropertyChanged(nameof(TreeViewDisplayName));
            OnObjectChanged();
        }
    }

    public List<AllowedDatatype> AllowedDatatypes
    {
        get => _allowedDatatypes;
        set
        {
            _allowedDatatypes = value;
            OnPropertyChanged();
        }
    }

    public override string TreeViewDisplayName => $"{DisplayName} : {AllowedDatatype.Text}";

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

    private void InitializeAllowedDatatypes()
    {
        _allowedDatatypes.Add(new AllowedDatatype { Text = "bigint", SqlTypeName = "bigint" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "bit", SqlTypeName = "bit" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "date", SqlTypeName = "date" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "decimal", SqlTypeName = "decimal" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "float", SqlTypeName = "float" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "int", SqlTypeName = "int" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "money", SqlTypeName = "money" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "nvarchar", SqlTypeName = "nvarchar(255)" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "real", SqlTypeName = "real" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "tinyint", SqlTypeName = "tinyint" });
        _allowedDatatypes.Add(new AllowedDatatype { Text = "varchar", SqlTypeName = "varchar(255)" });
        OnPropertyChanged("AllowedDatatypes");
    }

    private void CalcSQLText()
    {
        var newCreateType = string.Empty;
        var newExecType = string.Empty;

        var builder = new StringBuilder();

        newCreateType = createtype.Replace("__CREATETYPE__", Name);
        newCreateType = newCreateType.Replace("__DATATYPE__", AllowedDatatype.SqlTypeName);
        builder.AppendLine(newCreateType);
        builder.AppendLine("GO");


        newExecType += exectype.Replace("__CREATETYPE__", Name);
        newExecType = newExecType.Replace("__TYPEDISPLAYNAME__", DisplayName);
        builder.AppendLine(newExecType);
        builder.AppendLine("GO");
        SQLText = builder.ToString();
    }

    public WorkflowParameter ShallowCopy()
    {
        return new WorkflowParameter
        {
            Name = _name,
            DisplayName = _displayName,
            ParameterActionName = _parameterActionName,
            AllowedDatatype = _allowedDatatype,
            Identity = Identity
        };
    }

    [RelayCommand]
    public void CloseClicked()
    {
        if (OnRequestClose == null) return;

        OnRequestClose(this, EventArgs.Empty);
    }


    [RelayCommand]
    public void SaveClicked()
    {
        if (OnRequestExit == null) return;

        OnRequestExit(this, EventArgs.Empty);
    }
}

public class AllowedDatatype
{
    public string? Text { get; set; }
    public string? SqlTypeName { get; set; }
}