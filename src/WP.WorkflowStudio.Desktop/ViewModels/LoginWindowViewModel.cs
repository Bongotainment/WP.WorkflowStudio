using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WP.WorkflowStudio.DataAccess.SQLServer;
using WP.WorkflowStudio.Desktop.Settings;
using WP.WorkflowStudio.Repositories;

namespace WP.WorkflowStudio.Desktop.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    private readonly SQLServerConnection _connection;
    private readonly UserRepository _userRepository;

    [ObservableProperty] private string _dbName;

    [ObservableProperty] private string _dbPassword;

    [ObservableProperty] private string _dbUserName;

    [ObservableProperty] private bool _isAuthenticated;

    [ObservableProperty] private bool _isAuthorized;

    [ObservableProperty] private bool _isDBAuthorized;

    [ObservableProperty] private string _sqlServerAddress;

#pragma warning disable CS8618
    public LoginWindowViewModel()
    {
        _sqlServerAddress = string.Empty;
        _dbName = string.Empty;
        _dbUserName = string.Empty;
        _dbPassword = string.Empty;
    }
#pragma warning restore CS8618

    public LoginWindowViewModel(UserRepository userRepository, SQLServerConnection connection,
        IOptions<SqlSettings> settings) : this()
    {
        _userRepository = userRepository;
        _connection = connection;

        _sqlServerAddress = settings.Value.Address;
        _dbPassword = settings.Value.Password;
        _dbName = settings.Value.Database;
        _dbUserName = settings.Value.User;
    }

    public event EventHandler OnRequestClose;
    public event EventHandler OnRequestExit;

    [RelayCommand]
    public async Task LoginClicked()
    {
        _connection.Address = SqlServerAddress;
        _connection.Database = DbName;
        _connection.User = DbUserName;
        _connection.Password = DbPassword;
        IsDBAuthorized = await _connection.TestConnection();
        IsAuthenticated = IsDBAuthorized;
        IsAuthorized = IsDBAuthorized;
        if (IsAuthorized) OnRequestClose(this, EventArgs.Empty);
    }

    [RelayCommand]
    public void CloseClicked()
    {
        IsDBAuthorized = false;
        IsAuthenticated = false;
        OnRequestExit(this, EventArgs.Empty);
    }
}