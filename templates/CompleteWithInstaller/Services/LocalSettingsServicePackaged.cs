namespace CompleteWithInstaller.Services;

public sealed class LocalSettingsServicePackaged : ILocalSettingsService
{
    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out object? obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
        => ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
}
