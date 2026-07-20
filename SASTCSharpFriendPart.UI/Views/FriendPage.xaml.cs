using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

public sealed partial class FriendPage : Page
{
    public FriendViewModel ViewModel { get; } = new();

    public FriendPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EntranceAnimation.Begin();
    }
}
