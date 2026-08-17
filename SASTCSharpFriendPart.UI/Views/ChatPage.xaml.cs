using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// Displays the chat interface and plays its entrance animation when loaded.
/// </summary>
public sealed partial class ChatPage : Page
{
    public ChatViewModel ViewModel { get; } = new();

    public ChatPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    /// <summary>
    /// Starts the entrance animation after the page is ready.
    /// </summary>
    /// <param name="sender">触发事件的控件</param>
    /// <param name="e">事件参数</param>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EntranceAnimation.Begin();
    }
}
