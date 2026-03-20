using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CourseSellingApp.Views
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void OnTelegramClicked(object? sender, RoutedEventArgs e)
        {
            string url = "https://t.me/eighhty_six";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "xdg-open",
                        Arguments = url,
                        UseShellExecute = false
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "open",
                        Arguments = url,
                        UseShellExecute = false
                    });
                }
            }
        }
    }
}
