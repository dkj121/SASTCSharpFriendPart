using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

public sealed partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; } = new();

    public MainWindow()
    {
        InitializeComponent();

        // Configure custom title bar
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        AppWindow.SetIcon("Assets/AppIcon.ico");

        // Wire up navigation
        NavigationService.ContentFrame = RootFrame;

        // Wire up theme
        ThemeService.AppWindow = this;
        ThemeService.OnThemeChanged = UpdateNavigationButtonsColor;
        ThemeService.Initialize();

        // Navigate to welcome page on startup
        RootFrame.Navigate(typeof(WelcomePage));
    }

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

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        if (AppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
            presenter.Minimize();
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        if (AppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
        {
            if (presenter.State == Microsoft.UI.Windowing.OverlappedPresenterState.Maximized)
                presenter.Restore();
            else
                presenter.Maximize();
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
