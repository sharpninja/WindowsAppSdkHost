using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HostedWindowsAppSdk
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : INotifyPropertyChanged
    {
        private string _status;

        public MainWindow()
        {
            Title = "BenchmarkDotnet Workshop";

            this.InitializeComponent();

            Status = "Ready.";
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value ?? "";
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Status
    {
        public static void Set(string status)
        {
            App.MainWindow.Status = status;
        }
    }
}
