using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PlaygroundPage : Page
{
    public PlaygroundPage()
    {
        InitializeComponent();
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
