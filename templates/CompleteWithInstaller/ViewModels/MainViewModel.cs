namespace CompleteWithInstaller.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public sealed class MainViewModel : ObservableRecipient, INavigationAware
{
    // TODO: Set the default URL to display.
    private const string DEFAULT_URL = "https://docs.microsoft.com/windows/apps/";
    private Uri? _source;
    private bool _isLoading = true;
    private bool _hasFailures;
    private ICommand? _browserBackCommand;
    private ICommand? _browserForwardCommand;
    private ICommand? _reloadCommand;
    private ICommand? _retryCommand;

    public IWebViewService? WebViewService
    {
        get;
        set;
    }

    public Uri? Source
    {
        get => _source;
        set => SetProperty(ref _source, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool HasFailures
    {
        get => _hasFailures;
        set => SetProperty(ref _hasFailures, value);
    }

    public ICommand BrowserBackCommand
    {
        get => _browserBackCommand ??= new RelayCommand(
            () => WebViewService?.GoBack(),
            () => WebViewService?.CanGoBack ?? false
        );
        set => throw new NotImplementedException();
    }

    public ICommand BrowserForwardCommand
    {
        get => _browserForwardCommand ??= new RelayCommand(
            () => WebViewService?.GoForward(),
            () => WebViewService?.CanGoForward ?? false
        );
        set => throw new NotImplementedException();
    }

    public ICommand ReloadCommand
    {
        get => _reloadCommand ??= new RelayCommand(() => WebViewService?.Reload());
        set => throw new NotImplementedException();
    }

    public ICommand RetryCommand
    {
        get => _retryCommand ??= new RelayCommand(OnRetry);
        set => throw new NotImplementedException();
    }

    public ICommand? OpenInBrowserCommand
    {
        get;
        set;
    }

    public MainViewModel(IWebViewService webViewService)
    {
        WebViewService = webViewService;
    }

    public void OnNavigatedTo(object parameter)
    {
        if (WebViewService is not null)
        {
            WebViewService.NavigationCompleted += OnNavigationCompleted;
        }

        Source = new(MainViewModel.DEFAULT_URL);
    }

    public void OnNavigatedFrom()
    {
        WebViewService?.UnregisterEvents();

        if (WebViewService is not null)
        {
            WebViewService.NavigationCompleted -= OnNavigationCompleted;
        }
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        OnPropertyChanged(nameof(BrowserBackCommand));
        OnPropertyChanged(nameof(BrowserForwardCommand));
        if (webErrorStatus != default)
        {
            HasFailures = true;
        }
    }

    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }
}
