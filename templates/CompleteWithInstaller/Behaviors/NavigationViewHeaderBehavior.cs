using Microsoft.Xaml.Interactivity;

namespace CompleteWithInstaller.Behaviors;

public sealed class NavigationViewHeaderBehavior : Behavior<NavigationView>
{
    public DataTemplate? DefaultHeaderTemplate
    {
        get;
        set;
    }

    public object DefaultHeader
    {
        get => GetValue(DefaultHeaderProperty);
        set => SetValue(DefaultHeaderProperty, value);
    }

    private static NavigationViewHeaderBehavior? _current;

    public static readonly DependencyProperty HeaderContextProperty
        = DependencyProperty.RegisterAttached(
            "HeaderContext",
            typeof(object),
            typeof(NavigationViewHeaderBehavior),
            new(
                null,
                static (_, _) => _current?.UpdateHeader()
            )
        );

    public static readonly DependencyProperty HeaderModeProperty
        = DependencyProperty.RegisterAttached(
            "HeaderMode",
            typeof(bool),
            typeof(NavigationViewHeaderBehavior),
            new(
                defaultValue: NavigationViewHeaderMode.Always,
                static (_, _) => _current?.UpdateHeader()
            )
        );

    public static readonly DependencyProperty HeaderTemplateProperty
        = DependencyProperty.RegisterAttached(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(NavigationViewHeaderBehavior),
            new(
                null,
                static (_, _) => _current?.UpdateHeaderTemplate()
            )
        );

    // ReSharper disable once InconsistentNaming
    public static readonly DependencyProperty DefaultHeaderProperty = DependencyProperty.Register(
        "DefaultHeader",
        typeof(object),
        typeof(NavigationViewHeaderBehavior),
        new(
            null,
            static (_, _) => _current?.UpdateHeader()
        )
    );

    public static object GetHeaderContext(Page item)
        => item.GetValue(HeaderContextProperty);

    public static NavigationViewHeaderMode GetHeaderMode(Page item)
        => (NavigationViewHeaderMode)item.GetValue(
            HeaderModeProperty
        );

    public static DataTemplate GetHeaderTemplate(Page? item)
        => (DataTemplate)item?.GetValue(HeaderTemplateProperty)!;

    public static void SetHeaderContext(Page item, object value)
        => item.SetValue(HeaderContextProperty, value);

    public static void SetHeaderMode(Page item, NavigationViewHeaderMode value)
        => item.SetValue(HeaderModeProperty, value);

    public static void SetHeaderTemplate(Page item, DataTemplate value)
        => item.SetValue(HeaderTemplateProperty, value);

    protected override void OnAttached()
    {
        base.OnAttached();
        _current = this;
        var navigationService = App.GetService<INavigationService>();

        if (navigationService is not null)
        {
            navigationService.Navigated += OnNavigated;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        var navigationService = App.GetService<INavigationService>();

        if (navigationService is not null)
        {
            navigationService.Navigated -= OnNavigated;
        }
    }

    private Page? _currentPage;

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if ((sender as Frame) is not
            {
                Content: Page page,
            })
        {
            return;
        }

        _currentPage = page;

        UpdateHeader();
        UpdateHeaderTemplate();
    }

    private void UpdateHeader()
    {
        if (_currentPage == null)
        {
            return;
        }

        var headerMode = NavigationViewHeaderBehavior.GetHeaderMode(_currentPage);

        if (headerMode == NavigationViewHeaderMode.Never)
        {
            AssociatedObject.Header = null;
            AssociatedObject.AlwaysShowHeader = false;
        }
        else
        {
            var headerFromPage = NavigationViewHeaderBehavior.GetHeaderContext(_currentPage);
            AssociatedObject.Header = headerFromPage;

            AssociatedObject.AlwaysShowHeader = headerMode == NavigationViewHeaderMode.Always;
        }
    }

    private void UpdateHeaderTemplate()
    {
        if (_currentPage is null)
        {
            return;
        }

        var headerTemplate = GetHeaderTemplate(_currentPage);
        AssociatedObject.HeaderTemplate = headerTemplate;
    }
}
