namespace CompleteWithInstaller.Contracts.Services;

public interface INavigationViewService
{
    IList<object> MenuItems
    {
        get;
        set;
    }

    object SettingsItem
    {
        get;
        set;
    }

    void Initialize(NavigationView? navigationView);

    void UnRegisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);
}
