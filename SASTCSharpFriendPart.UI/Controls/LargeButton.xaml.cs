using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SASTCSharpFriendPart_UI.Controls;

public sealed partial class LargeButton : UserControl
{
    public static readonly DependencyProperty MainTitleProperty =
        DependencyProperty.Register(nameof(MainTitle), typeof(string), typeof(LargeButton), new PropertyMetadata(""));

    public string MainTitle
    {
        get => (string)GetValue(MainTitleProperty);
        set => SetValue(MainTitleProperty, value);
    }

    public static readonly DependencyProperty SubTitleProperty =
        DependencyProperty.Register(nameof(SubTitle), typeof(string), typeof(LargeButton), new PropertyMetadata(""));

    public string SubTitle
    {
        get => (string)GetValue(SubTitleProperty);
        set => SetValue(SubTitleProperty, value);
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(object), typeof(LargeButton), new PropertyMetadata(null));

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public event RoutedEventHandler? Click;

    public LargeButton()
    {
        InitializeComponent();
    }

    private void MainButton_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
