namespace CompleteWithInstaller.Contracts.Services;

public interface IWebViewService
{
    bool CanGoBack
    {
        get;
        set;
    }

    bool CanGoForward
    {
        get;
        set;
    }

    event EventHandler<CoreWebView2WebErrorStatus> NavigationCompleted;

    void Initialize(WebView2 webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}
