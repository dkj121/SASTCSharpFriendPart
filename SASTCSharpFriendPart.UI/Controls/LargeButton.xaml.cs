using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SASTCSharpFriendPart_UI.Controls;

/// <summary>
/// LargeButton 控件用于显示一个大型按钮，包含主标题、副标题和图标。
/// </summary>
public sealed partial class LargeButton : UserControl
{
    /// <summary>
    /// MainTitle 属性用于绑定按钮的主标题文本。
    /// </summary>
    /// <param name="PropertyMetadata("")"></param>
    /// <returns></returns>
    public static readonly DependencyProperty MainTitleProperty =
        DependencyProperty.Register(nameof(MainTitle), typeof(string), typeof(LargeButton), new PropertyMetadata(""));

    /// <summary>
    /// 主标题文本属性，用于显示按钮的主要信息。
    /// </summary>
    public string MainTitle
    {
        get => (string)GetValue(MainTitleProperty);
        set => SetValue(MainTitleProperty, value);
    }

    /// <summary>
    /// 副标题文本属性，用于显示按钮的次要信息。
    /// </summary>
    /// <param name="PropertyMetadata("")"></param>
    /// <returns></returns>
    public static readonly DependencyProperty SubTitleProperty =
        DependencyProperty.Register(nameof(SubTitle), typeof(string), typeof(LargeButton), new PropertyMetadata(""));

    /// <summary>
    /// 副标题文本属性，用于显示按钮的次要信息。
    /// </summary>
    public string SubTitle
    {
        get => (string)GetValue(SubTitleProperty);
        set => SetValue(SubTitleProperty, value);
    }

    /// <summary>
    /// 图标属性，用于显示按钮的图标。
    /// </summary>
    /// <param name="PropertyMetadata(null)"></param>
    /// <returns></returns>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(object), typeof(LargeButton), new PropertyMetadata(null));

    /// <summary>
    /// 图标属性，用于显示按钮的图标。
    /// </summary>
    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 点击事件，用于处理按钮点击操作。
    /// </summary>
    public event RoutedEventHandler? Click;

    /// <summary>
    /// 创建一个新的 LargeButton 实例。
    /// </summary>
    public LargeButton()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 点击按钮时触发的事件处理程序，调用 Click 事件。
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">事件参数</param>
    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
