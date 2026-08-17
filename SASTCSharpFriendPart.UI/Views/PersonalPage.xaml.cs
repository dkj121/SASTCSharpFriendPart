using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// 个人信息页面，负责显示和编辑当前用户的本地个人资料。
/// </summary>
public sealed partial class PersonalPage : Page
{
    public PersonalViewModel ViewModel { get; } = new();

    public PersonalPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    /// <summary>
    /// 页面加载完成后播放入口动画。
    /// </summary>
    /// <param name="sender">触发事件的控件</param>
    /// <param name="e">事件参数</param>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EntranceAnimation.Begin();
    }
}
