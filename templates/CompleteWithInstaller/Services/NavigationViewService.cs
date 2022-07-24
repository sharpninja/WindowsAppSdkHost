namespace CompleteWithInstaller.Services;

[ System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0007:Use implicit type",
    Justification = "<Pending>"
) ]
public sealed class NavigationViewService : INavigationViewService
{
    private readonly INavigationService _navigationService;

    private readonly IPageService _pageService;

    private NavigationView? _navigationView;

    public IList<object> MenuItems
    {
        get => _navigationView?.MenuItems ?? new List<object>();
        set => throw new NotImplementedException();
    }

    public object SettingsItem
    {
        get => _navigationView?.SettingsItem is not null
            ? _navigationView?.SettingsItem!
            : default!;
        set => throw new NotImplementedException();
    }

    public NavigationViewService(INavigationService navigationService, IPageService pageService)
    {
        _navigationService = navigationService;
        _pageService = pageService;
    }

    public void Initialize(NavigationView? navigationView)
    {
        _navigationView = navigationView;

        if (_navigationView is null)
        {
            return;
        }

        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
    }

    public void UnRegisterEvents()
    {
        if (_navigationView is null)
        {
            return;
        }

        _navigationView.BackRequested -= OnBackRequested;
        _navigationView.ItemInvoked -= OnItemInvoked;
    }

    public NavigationViewItem? GetSelectedItem(Type pageType)
        => GetSelectedItem(_navigationView?.MenuItems, pageType);

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        => _navigationService.GoBack();

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            string? fullName = typeof(SettingsViewModel).FullName;

            if (fullName is not null)
            {
                _navigationService.NavigateTo(fullName);
            }
        }
        else
        {
            NavigationViewItem? selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper._navigateToProperty) is string pageKey)
            {
                _navigationService.NavigateTo(pageKey);
            }
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object>? menuItems, Type pageType)
    {
        if (menuItems is null)
        {
            return null;
        }

        foreach (NavigationViewItem item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
            {
                return item;
            }

            NavigationViewItem? selectedChild = GetSelectedItem(item.MenuItems, pageType);

            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper._navigateToProperty) is string pageKey)
        {
            return _pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }
}
