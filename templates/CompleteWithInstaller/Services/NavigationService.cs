// ReSharper disable VariableCanBeNotNullable
namespace CompleteWithInstaller.Services;

// For more information on navigation between pages see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
[ System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0007:Use implicit type",
    Justification = "<Pending>"
) ]
public sealed class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    public Frame Frame
    {
        get
        {
            if (_frame != null)
            {
                return _frame;
            }

            _frame = App.MainWindow.Content as Frame;
            RegisterFrameEvents();

            return _frame!;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    public bool CanGoBack
    {
        get => Frame.CanGoBack;
        set => throw new NotImplementedException();
    }

    public NavigationService(IPageService pageService)
        => _pageService = pageService;

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    public bool GoBack()
    {
        if (_frame is null ||
            !CanGoBack)
        {
            return false;
        }

        object? vmBeforeNavigation = _frame.GetPageViewModel();

        _frame.GoBack();

        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        return true;

    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        Type? pageType = _pageService.GetPageType(pageKey);

        if (_frame is null ||
            ((_frame.Content?.GetType() == pageType) &&
             ((parameter == null) || parameter.Equals(_lastParameterUsed))))
        {
            return false;
        }

        _frame.Tag = clearNavigation;
        object? vmBeforeNavigation = _frame.GetPageViewModel();
        bool navigated = _frame.Navigate(pageType, parameter);

        if (!navigated)
        {
            return navigated;
        }

        _lastParameterUsed = parameter;
        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        return navigated;

    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame)
        {
            return;
        }

        bool clearNavigation = (bool)frame.Tag;
        if (clearNavigation)
        {
            frame.BackStack.Clear();
        }

        if (frame.GetPageViewModel() is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(e.Parameter);
        }

        Navigated?.Invoke(sender, e);
    }
}
