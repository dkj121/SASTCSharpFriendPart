using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// MainWindow 类是应用程序的主窗口，负责管理导航、主题设置和窗口操作（最小化、最大化、关闭）。
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; } = new();

    private bool _isUpdatingNavigationSelection;

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
        ThemeService.OnThemeChanged = UpdateNavigationVisuals;
        ThemeService.Initialize();

        // 导航到欢迎页面
        RootFrame.Navigate(typeof(WelcomePage));
    }

    /// <summary>
    /// 更新标题栏文字颜色。NavigationView 的颜色由 WinUI 主题资源自动管理。
    /// </summary>
    public void UpdateNavigationVisuals()
    {
        var textColor = ThemeService.IsAcrylicMode
            ? Microsoft.UI.Colors.White
            : (ThemeService.IsDarkTheme() ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.Black);

        TitleTextBlock.Foreground = new SolidColorBrush(textColor);
    }

    private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (_isUpdatingNavigationSelection || args.SelectedItem is not NavigationViewItem { Tag: string pageName })
            return;

        ViewModel.NavigateCommand.Execute(pageName);
    }

    private void RootFrame_Navigated(object sender, NavigationEventArgs e)
    {
        var pageName = e.SourcePageType.Name;
        var selectedItem = MainNavigationView.MenuItems
            .Concat(MainNavigationView.FooterMenuItems)
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag is string tag && tag == pageName);

        _isUpdatingNavigationSelection = true;
        MainNavigationView.SelectedItem = selectedItem;
        _isUpdatingNavigationSelection = false;
    }
}
