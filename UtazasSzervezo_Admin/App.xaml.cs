using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using UtazasSzervezo_Admin.Services;
using UtazasSzervezo_Admin.Views;

namespace UtazasSzervezo_Admin;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri("https://your-api-base-url.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddTransient<MainWindow>();
    }
}

