using System.Runtime.InteropServices;

using Windows.UI;

using Microsoft.UI;

using WinRT.Interop;

namespace CompleteWithInstaller.Helpers;

// Helper class to workaround custom title bar bugs.
// DISCLAIMER: The resource key names and color values used below are subject to change. Do not depend on them.
// https://github.com/microsoft/TemplateStudio/issues/4516
internal class TitleBarHelper
{
    private const int WAINACTIVE = 0x00;
    private const int WAACTIVE = 0x01;
    private const int WMACTIVATE = 0x0006;

    [DllImport("user32.dll")]
    extern private static IntPtr GetActiveWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    extern private static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    public static void UpdateTitleBar(ElementTheme theme)
    {
        switch (App.MainWindow.ExtendsContentIntoTitleBar)
        {
            case true:
            {
                if (theme != ElementTheme.Default)
                {
                    Application.Current.Resources["WindowCaptionForeground"] = theme switch
                    {
                        ElementTheme.Dark => new(Colors.White),
                        ElementTheme.Light => new(Colors.Black),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };

                    Application.Current.Resources["WindowCaptionForegroundDisabled"] = theme switch
                    {
                        ElementTheme.Dark => new(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                        ElementTheme.Light => new(Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };

                    Application.Current.Resources["WindowCaptionButtonBackgroundPointerOver"] = theme switch
                    {
                        ElementTheme.Dark => new(Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF)),
                        ElementTheme.Light => new(Color.FromArgb(0x33, 0x00, 0x00, 0x00)),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };

                    Application.Current.Resources["WindowCaptionButtonBackgroundPressed"] = theme switch
                    {
                        ElementTheme.Dark => new(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                        ElementTheme.Light => new(Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };

                    Application.Current.Resources["WindowCaptionButtonStrokePointerOver"] = theme switch
                    {
                        ElementTheme.Dark => new(Colors.White),
                        ElementTheme.Light => new(Colors.Black),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };

                    Application.Current.Resources["WindowCaptionButtonStrokePressed"] = theme switch
                    {
                        ElementTheme.Dark => new(Colors.White),
                        ElementTheme.Light => new(Colors.Black),
                        _ => new SolidColorBrush(Colors.Transparent),
                    };
                }

                Application.Current.Resources["WindowCaptionBackground"] = new SolidColorBrush(Colors.Transparent);
                Application.Current.Resources["WindowCaptionBackgroundDisabled"] = new SolidColorBrush(Colors.Transparent);

                IntPtr hwnd = WindowNative.GetWindowHandle(App.MainWindow);
                if (hwnd == TitleBarHelper.GetActiveWindow())
                {
                    TitleBarHelper.SendMessage(hwnd, TitleBarHelper.WMACTIVATE, TitleBarHelper.WAINACTIVE, IntPtr.Zero);
                    TitleBarHelper.SendMessage(hwnd, TitleBarHelper.WMACTIVATE, TitleBarHelper.WAACTIVE, IntPtr.Zero);
                }
                else
                {
                    TitleBarHelper.SendMessage(hwnd, TitleBarHelper.WMACTIVATE, TitleBarHelper.WAACTIVE, IntPtr.Zero);
                    TitleBarHelper.SendMessage(hwnd, TitleBarHelper.WMACTIVATE, TitleBarHelper.WAINACTIVE, IntPtr.Zero);
                }

                break;
            }
        }
    }
}
