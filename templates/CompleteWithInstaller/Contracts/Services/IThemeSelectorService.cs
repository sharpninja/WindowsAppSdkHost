namespace CompleteWithInstaller.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
        set;
    }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
