namespace CompleteWithInstaller.Helpers;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public sealed class NavigationHelper
{
    public static string Get_navigateTo(NavigationViewItem item) => (string)item.GetValue(NavigationHelper._navigateToProperty);

    public static void Set_navigateTo(NavigationViewItem item, string value) => item.SetValue(NavigationHelper._navigateToProperty, value);

    public static readonly DependencyProperty _navigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new(null));
}
