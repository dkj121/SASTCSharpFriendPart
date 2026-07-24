using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// FriendPage 类是一个页面类，负责显示好友相关的界面，并在加载时播放入口动画。
/// </summary>
public sealed partial class FriendPage : Page
{
    public FriendViewModel ViewModel { get; } = new();

    public FriendPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    /// <summary>
    /// OnLoaded 方法在页面加载完成后触发，负责启动入口动画。
    /// </summary>
    /// <param name="sender">触发事件的控件</param>
    /// <param name="e">事件参数</param>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EntranceAnimation.Begin();
    }
}
