// ReSharper disable LocalizableElement
namespace CompleteWithInstaller.Services;

[ System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0007:Use implicit type",
    Justification = "<Pending>"
) ]
public sealed class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<SettingsViewModel, SettingsPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<TVm, TV>()
        where TVm : ObservableObject
        where TV : Page
    {
        lock (_pages)
        {
            string? key = typeof(TVm).FullName;

            if (key is null)
            {
                return;
            }

            if (_pages.ContainsKey(key!))
            {
                return;
            }

            Type type = typeof(TV);
            if (_pages.Any(p => p.Value == type))
            {
                return;
            }

            _pages.Add(key!, type);
        }
    }
}
