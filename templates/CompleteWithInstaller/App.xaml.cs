

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace CompleteWithInstaller;

public sealed partial class App
{
    private static Window? _windows;
    private static ILogger<App>? _logger;
    private static IConfiguration? _config;

    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public static T? GetService<T>()
        where T : class
        => Host.Services.GetService<T>();

    public static object? GetService(Type type)
        => Host.Services.GetService(type);

    public static T GetRequiredService<T>()
        where T : class
        => Host.Services.GetRequiredService<T>();

    public static object GetRequiredService(Type type)
        => Host.Services.GetRequiredService(type);

    public static IHost Host
    {
        get;
        internal set;
    } = null!;

    public static Window MainWindow
    {
        get => _windows ??= GetRequiredService<Window>();
        set => throw new NotImplementedException();
    }

    public static ILogger Logger
    {
        get => _logger ??= GetRequiredService<ILogger<App>>();
        set => throw new NotImplementedException();
    }

    public static IConfiguration Configuration
    {
        get => _config ??= GetRequiredService<IConfiguration>();
        set => throw new NotImplementedException();
    }

    public App()
    {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        => Logger.LogError(e.Exception, e.Message);

    // ReSharper disable once AsyncVoidMethod
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        IActivationService activationService = GetRequiredService<IActivationService>();
        await activationService.ActivateAsync(args);
    }
}
