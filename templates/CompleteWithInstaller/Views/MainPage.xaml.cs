namespace CompleteWithInstaller.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/
public sealed partial class MainPage
{
    public MainViewModel? ViewModel
    {
        get;
        set;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        ViewModel?.WebViewService?.Initialize(WebView);
    }
}
