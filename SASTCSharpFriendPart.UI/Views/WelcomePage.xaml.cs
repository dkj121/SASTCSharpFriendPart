using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// WelcomePage 类是应用程序的欢迎页面，负责显示欢迎信息和引导用户进行下一步操作。
/// </summary>
public sealed partial class WelcomePage : Page
{
    public WelcomeViewModel ViewModel { get; } = new();

    public WelcomePage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        HeroAnimation.Begin();
        MainTextAnimation.Begin();
        LogoAnimation.Begin();
        SubTextAnimation.Begin();
        HelpButtonAnimation.Begin();
    }

    /// <summary>
    /// HelpButton_Click 方法在用户点击帮助按钮时触发，导航到 MoreInfoPage 页面以显示更多信息。
    /// </summary>
    /// <param name="sender">触发事件的控件</param>
    /// <param name="e">事件参数</param>
    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(typeof(MoreInfoPage));
    }
}
