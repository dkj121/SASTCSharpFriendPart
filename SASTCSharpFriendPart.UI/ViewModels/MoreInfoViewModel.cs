using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class MoreInfoViewModel : ObservableObject
{
    [RelayCommand]
    private async Task OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return;

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
