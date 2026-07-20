using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

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

    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(typeof(MoreInfoPage));
    }
}
