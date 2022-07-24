#pragma warning disable CS8892

namespace HostedWindowsAppSdk;

public static class Program
{
    private static IHostBuilder CreateBuilder(string[] args) => new WindowsAppSdkHostBuilder<App>(args)
        .ConfigureServices(
            static (context, services) =>
            {
                // Default Activation Handler
                services
                    .AddTransient<ActivationHandler<LaunchActivatedEventArgs>,
                        DefaultActivationHandler>();

                // Other Activation Handlers

                // Services
                services.AddSingleton<ILocalSettingsService, LocalSettingsServicePackaged>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddTransient<IWebViewService, WebViewService>();
                services.AddTransient<INavigationViewService, NavigationViewService>();

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IFileService, FileService>();

                // Views and ViewModels
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MainPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();

                // Configuration
                services.Configure<LocalSettingsOptions>(
                    context.Configuration.GetSection(nameof(LocalSettingsOptions))
                );
            }
        );

    [STAThread]
    private static void Main(string[] args)
    {
        IHostBuilder builder = CreateBuilder(args);

        // Your configuration Goes Here.
        builder.ConfigureServices(
            static collection =>
            {
                collection.AddSingleton(
                    static _ => new Window
                    {
                        Title = "AppDisplayName".GetLocalized(),
                    }
                );
            }
        );

        App.Host = builder.Build();

        App.Host.StartAsync()
            .GetAwaiter()
            .GetResult();
    }
}