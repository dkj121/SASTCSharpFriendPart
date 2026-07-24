using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// MoreInfoViewModel 类是一个视图模型，用于管理更多详细信息页面的交互逻辑。
/// </summary>
public partial class MoreInfoViewModel : ObservableObject
{
    /// <summary>
    /// OpenUrl 方法用于在默认浏览器中打开指定的 URL，根据不同的操作系统使用相应的命令来启动浏览器。
    /// </summary>
    /// <param name="url">要打开的 URL</param>
    /// <returns></returns>
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
