using Windows.ApplicationModel;

// ReSharper disable AsyncVoidLambda

namespace CompleteWithInstaller.ViewModels;

public sealed class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    private string? _versionDescription;

    public string? VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    private ICommand? _switchThemeCommand;

    public ICommand SwitchThemeCommand
    {
        get => _switchThemeCommand ??= new RelayCommand<ElementTheme>(
            async param =>
            {
                if (ElementTheme == param)
                {
                    return;
                }

                ElementTheme = param;
                await _themeSelectorService.SetThemeAsync(param);
            }
        );
        set => throw new NotImplementedException();
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        VersionDescription = GetVersionDescription();
    }

    private static string GetVersionDescription()
    {
        string appName = "AppDisplayName".GetLocalized();
        PackageVersion version = Package.Current.Id.Version;

        return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
