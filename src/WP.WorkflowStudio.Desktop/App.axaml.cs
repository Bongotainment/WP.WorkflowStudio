using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using WP.WorkflowStudio.DataAccess.CustomWorkflows;
using WP.WorkflowStudio.DataAccess.DBO;
using WP.WorkflowStudio.DataAccess.SQLServer;
using WP.WorkflowStudio.DataAccess.Workflows;
using WP.WorkflowStudio.Desktop.Services;
using WP.WorkflowStudio.Desktop.Settings;
using WP.WorkflowStudio.Desktop.ViewModels;
using WP.WorkflowStudio.Desktop.ViewModels.CustomWorkflows;
using WP.WorkflowStudio.Desktop.Views;
using WP.WorkflowStudio.Repositories;

namespace WP.WorkflowStudio.Desktop;

public class App : Application
{
    public static IHost? AppHost { get; private set; }

    public override void Initialize()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configHost => { configHost.AddEnvironmentVariables("DOTNET_"); })
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();
                var env = hostingContext.HostingEnvironment;
                var environment = env.EnvironmentName;
#if DEBUG
                environment = "Development"; // Set a default environment in debug mode!
#endif

                configuration.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true);
                var configurationRoot = configuration.Build();
            })
            .ConfigureServices((hostContext, services) =>
            {
                //Settings
                services.Configure<SqlSettings>(hostContext.Configuration.GetSection("Sql"));
                //ViewModels
                services.TryAddTransient<MainWindow>();
                services.TryAddTransient<MainWindowViewModel>();
                services.TryAddTransient<LoginWindowViewModel>();
                //Dataaccess
                services.TryAddTransient<WorkflowAccess>();
                services.TryAddTransient<WorkflowAktionAccess>();
                services.TryAddTransient<WorkflowBedingungAccess>();
                services.TryAddTransient<WorkflowEigenschaftAccess>();
                services.TryAddTransient<WorkflowEventAccess>();
                services.TryAddTransient<WorkflowEventGroupAccess>();
                services.TryAddTransient<JTLVersionAccess>();
                services.TryAddTransient<RMStatusAccess>();
                services.TryAddTransient<RMStatusSpracheAccess>();
                services.TryAddTransient<RMStatusWorkflowAccess>();
                services.TryAddTransient<BenutzerAccess>();
                services.TryAddTransient<WorkflowLogAccess>();
                services.TryAddTransient<WorkflowObjectsAccess>();
                services.TryAddTransient<CustomActionAccess>();
                services.TryAddTransient<AllowedDataTypesAccess>();
                //Services
                services.TryAddTransient<DialogService>();
                services.TryAddTransient<ProfileService>();
                services.TryAddTransient<WorkflowColorService>();
                services.TryAddTransient<WawiDbDocumentationService>();
                //Repositories
                services.TryAddTransient<UserRepository>();
                services.TryAddTransient<VersionRepository>();
                services.TryAddTransient<WorkflowRepository>();
                services.TryAddSingleton<SQLServerConnection>();
                services.TryAddSingleton<CustomWorkflowRepository>();
                services.TryAddTransient<WorkflowSqlViewModel>();
            })
            .Build();

        AvaloniaXamlLoader.Load(this);
    }


    public override async void OnFrameworkInitializationCompleted()
    {
        await AppHost!.StartAsync();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            //see https://docs.avaloniaui.net/docs/data-binding/data-validation#manage-validationplugins

            // ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);

            var startupForm = AppHost!.Services!.GetService(typeof(MainWindow)) as MainWindow;
            if (startupForm == null) throw new NullReferenceException("Startup Form couldn't be initialized properly");

            DialogService.RegisterMainWindow(startupForm);
            desktop.MainWindow = startupForm;
        }

        base.OnFrameworkInitializationCompleted();
    }
}