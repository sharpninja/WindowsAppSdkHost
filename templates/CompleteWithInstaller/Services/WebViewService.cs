namespace CompleteWithInstaller.Services;

public sealed class WebViewService : IWebViewService
{
    private WebView2 _webView = null!;

    public bool CanGoBack
    {
        get => _webView.CanGoBack;
        set => _webView.CanGoBack = value;
    }

    public bool CanGoForward
    {
        get => _webView.CanGoForward;
        set => _webView.CanGoForward = value;
    }

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    public void Initialize(WebView2 webView)
    {
        _webView = webView;
        _webView.NavigationCompleted += OnWebViewNavigationCompleted;
    }

    public void GoBack() => _webView.GoBack();

    public void GoForward() => _webView.GoForward();

    public void Reload() => _webView.Reload();

    public void UnregisterEvents() => _webView.NavigationCompleted -= OnWebViewNavigationCompleted;

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
