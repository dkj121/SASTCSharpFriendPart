using Microsoft.UI.Xaml.Controls;

using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

/// <summary>
/// SettingsPage 类是一个页面类，负责显示应用程序的设置界面。
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; } = new();

    public SettingsPage()
    {
        InitializeComponent();
    }
}
