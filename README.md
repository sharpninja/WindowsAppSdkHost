# Windows App SDK Host

Allows hosting a Windows App SDK Application in an `IHost` that manages the lifecycle of the hosted Application.

[![Packages](https://github.com/sharpninja/WindowsAppSdkHost/actions/workflows/packages.yml/badge.svg)](https://github.com/sharpninja/WindowsAppSdkHost/actions/workflows/packages.yml)

## Usage

__(Convert existing project or the default template's output)__

1. Add `<DefineConstants>DISABLE_XAML_GENERATED_MAIN</DefineConstants>` in the main `PropertyGroup` of your applications project file.
2. Add reference to `SparpNinja.Extensions.Hosting.WindowsAppSdk`
3. Add `Program.cs` to the root of your application project.
4. Add this code to the `Program.cs`:

```csharp
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = new WindowsAppSdkHostBuilder<App>();

        builder.ConfigureServices(
            (_, collection) =>
            {
                // If your main Window is named differently, change it here.
                collection.AddSingleton<MainWindow>();
            }
        );

        var app = builder.Build();

        app.StartAsync().GetAwaiter().GetResult();
    }
}
```

5. Set your `Program.cs` as the startup object by adding `<StartupObject>HostedWindowsAppSdk.Program</StartupObject>` to your project file.
6. Use the `CancelableApplication` as the base class of your application by modifying your `App.xaml`:

```xml
<host:CancelableApplication
    x:Class="HostedWindowsAppSdk.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:host="using:CommunityToolkit.Extensions.Hosting"
    xmlns:local="using:HostedWindowsAppSdk">
    <Application.Resources>
    </Application.Resources>
</host:CancelableApplication>
```

7. Update your App.xaml.cs to use dependency injection.

```csharp
protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
{
    // Get window from Dependency Injection.
    _mWindow = Services.GetRequiredService<MainWindow>();


    _mWindow.Activate();
}
```

## Notes

The `WindowsAppSdkHost` uses several features of the Microsoft.Extensions ecosystem:

1. Includes all configuration resources defined for the `DefaultHostBuilder`.
2. Registers the required `CancellableApplication` with dependency injection.
3. Manages the lifecycle of the Application in the `StartAsync` method of the `WindowsAppSdkHost`.
4. Write unhandled errors to default `ILogger`.  The `Ilogger` can be obtained from the static `Services` property of the `CancellableApplication` after building the app.

If there are other patterns you feel should be available or required then start a discussion.
