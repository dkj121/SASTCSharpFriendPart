using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// MainWindow 类是应用程序的主窗口，负责管理导航、主题设置和窗口操作（最小化、最大化、关闭）。
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; } = new();

    public MainWindow()
    {
        InitializeComponent();

        // 设置窗口扩展到标题栏，并设置自定义标题栏
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        AppWindow.SetIcon("Assets/AppIcon.ico");

        // 设置导航服务的内容框架为 RootFrame，以便在应用程序中进行页面导航
        NavigationService.ContentFrame = RootFrame;

        // 初始化主题服务，并设置主题更改时的回调函数
        ThemeService.AppWindow = this;
        ThemeService.OnThemeChanged = UpdateNavigationButtonsColor;
        ThemeService.Initialize();

        // 导航到欢迎页面
        RootFrame.Navigate(typeof(WelcomePage));
    }

    /// <summary>
    /// 更新导航按钮的颜色，根据当前主题（暗色模式或亮色模式）调整按钮的前景色。
    /// </summary>
    public void UpdateNavigationButtonsColor()
    {
        var textColor = ThemeService.IsAcrylicMode
            ? Microsoft.UI.Colors.White
            : (ThemeService.IsDarkTheme() ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.Black);

        var brush = new SolidColorBrush(textColor);
        WelcomeButton.Foreground = brush;
        FriendButton.Foreground = brush;
        SettingsButton.Foreground = brush;
        TitleTextBlock.Foreground = brush;
    }
}
