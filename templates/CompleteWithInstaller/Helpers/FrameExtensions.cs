namespace CompleteWithInstaller.Helpers;

public static class FrameExtensions
{
    public static object? GetPageViewModel(this Frame? frame) => frame != null
        ? frame.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null)
        : null;
}
