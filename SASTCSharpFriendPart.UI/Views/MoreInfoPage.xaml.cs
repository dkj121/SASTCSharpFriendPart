using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SASTCSharpFriendPart_UI.ViewModels;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SASTCSharpFriendPart_UI.Views;

public sealed partial class MoreInfoPage : Page
{
    public MoreInfoViewModel ViewModel { get; } = new();

    public MoreInfoPage()
    {
        InitializeComponent();
    }

    private async void Link_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.Tag is string url && !string.IsNullOrEmpty(url))
        {
            try
            {
                var uri = new Uri(url);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = uri.AbsoluteUri,
                        UseShellExecute = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", uri.AbsoluteUri);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", uri.AbsoluteUri);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Cannot open URL: {ex.Message}");
            }
        }
    }
}
