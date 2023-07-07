using System;
using System.Threading.Tasks;
using WP.WorkflowStudio.Desktop.ViewModels;
using WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;
using WP.WorkflowStudio.Desktop.Views;
using WP.WorkflowStudio.Desktop.Views.CustomWorkflows.Dialogs;

namespace WP.WorkflowStudio.Desktop.Services;

public class DialogService
{
    private static MainWindow? _window;
    private LoginWindow? _currentLoginWindow;

    public static void RegisterMainWindow(MainWindow window)
    {
        _window = window;
    }

    public async Task<bool> ShowLoginDialog(LoginWindowViewModel viewmodel)
    {
        var window = new LoginWindow
        {
            DataContext = viewmodel
        };
        viewmodel.OnRequestClose += OnLoginRequestClose;
        viewmodel.OnRequestExit += OnRequestExit;
        _currentLoginWindow = window;
        if (_window != null) await window.ShowDialog(_window);
        return viewmodel.IsAuthenticated;
    }

    public async Task<bool> ShowParameterDialog(object viewmodel)
    {
        var returnValue = false;
        if (viewmodel is WorkflowParameter parameter)
        {
            var window = new AddParameterDialog
            {
                DataContext = parameter
            };

            parameter.OnRequestClose += (sender, args) =>
            {
                if (window == null) return;

                window.Close();
            };
            parameter.OnRequestExit += (sender, args) =>
            {
                if (window == null) return;

                returnValue = true;
                window.Close();
            };
            if (_window != null) await window.ShowDialog(_window);
        }

        return returnValue;
    }

    public async Task<bool> ShowProcedureDialog(object viewmodel)
    {
        var returnValue = false;
        if (viewmodel is WorkflowProcedure procedure)
        {
            var window = new AddProcedureDialog
            {
                DataContext = procedure
            };

            procedure.OnRequestClose += (sender, args) =>
            {
                if (window == null) return;
                window.Close();
            };
            procedure.OnRequestExit += (sender, args) =>
            {
                if (window == null) return;

                returnValue = true;
                window.Close();
            };
            if (_window != null) await window.ShowDialog(_window);
        }

        return returnValue;
    }

    private void OnLoginRequestClose(object? s, EventArgs e)
    {
        _currentLoginWindow?.Close();
        var viewModel = (LoginWindowViewModel?)_currentLoginWindow?.DataContext;
        if (viewModel != null) viewModel.OnRequestClose -= OnLoginRequestClose;
        _currentLoginWindow = null;
    }

    private void OnRequestExit(object? s, EventArgs e)
    {
        if (_currentLoginWindow != null && _window != null)
        {
            _currentLoginWindow.Close();
            _window.Close();
        }
    }
}