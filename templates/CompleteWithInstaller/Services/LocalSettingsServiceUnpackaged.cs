using Microsoft.Extensions.Options;

namespace CompleteWithInstaller.Services;

public class LocalSettingsServiceUnpackaged : ILocalSettingsService
{
    private readonly IFileService _fileService;
    private readonly LocalSettingsOptions _options;
    private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private IDictionary<string, object>? _settings;

    public LocalSettingsServiceUnpackaged(
        IFileService fileService,
        IOptions<LocalSettingsOptions> options
    )
    {
        _fileService = fileService;
        _options = options.Value;
    }

    private async Task InitializeAsync()
    {
        if (_settings is null)
        {
            if (_options.ApplicationDataFolder is not null &&
                _options.LocalSettingsFile is not null)
            {
                string folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);

                string fileName = _options.LocalSettingsFile;
                _settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(folderPath, fileName)) ?? new Dictionary<string, object>();
            }
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        await InitializeAsync();

        if (_settings is not null && _settings.TryGetValue(key, out var obj))
        {
            return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0007:Use implicit type", Justification = "<Pending>")]
    public async Task SaveSettingAsync<T>(string key, T value)
    {
        await InitializeAsync();

        if (_settings is not null)
        {
            _settings[key] = await Json.StringifyAsync(value);

            if (_options.ApplicationDataFolder is not null &&
                _options.LocalSettingsFile is not null)
            {
                string folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);

                string fileName = _options.LocalSettingsFile;
                await Task.Run(() => _fileService.Save(folderPath, fileName, _settings));
            }
        }
    }
}
