using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using SASTCSharpFriendPart_UI.ViewModels;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// MoreInfoPage 类是一个页面类，负责显示应用程序的更多信息，包括参考资料、联系方式和二维码。
/// </summary>
public sealed partial class MoreInfoPage : Page
{
    public MoreInfoViewModel ViewModel { get; } = new();

    public MoreInfoPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Link_Tapped 方法在用户点击链接时触发，尝试在默认浏览器中打开指定的 URL。
    /// </summary>
    /// <param name="sender">触发事件的控件</param>
    /// <param name="e">事件参数</param>
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
