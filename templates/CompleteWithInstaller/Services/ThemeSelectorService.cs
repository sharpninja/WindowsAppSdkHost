namespace CompleteWithInstaller.Services;

public sealed class ThemeSelectorService : IThemeSelectorService
{
    private const string SETTINGS_KEY = "AppBackgroundRequestedTheme";

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    private readonly ILocalSettingsService _localSettingsService;

    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        string? themeName
            = await _localSettingsService.ReadSettingAsync<string>(
                ThemeSelectorService.SETTINGS_KEY
            );

        return Enum.TryParse(themeName, out ElementTheme cacheTheme)
            ? cacheTheme
            : ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
        => await _localSettingsService.SaveSettingAsync(ThemeSelectorService.SETTINGS_KEY, theme.ToString());
}
