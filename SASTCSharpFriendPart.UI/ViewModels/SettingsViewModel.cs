using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SASTCSharpFriendPart_UI.Services;

using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// SettingsViewModel 类是一个视图模型，用于管理应用程序的设置页面，包括主题选择和帮助导航功能。
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private bool _initializing;

    [ObservableProperty]
    private ObservableCollection<string> themes = new() { "明亮", "黑暗", "跟随系统设置", "亚克力" };

    [ObservableProperty]
    private string selectedTheme;

    public IRelayCommand HelpCommand { get; }
    /// <summary>
    /// SettingsViewModel 构造函数用于初始化 SettingsViewModel 实例，设置初始主题选择，并创建帮助命令以导航到更多信息页面。
    /// </summary>
    public SettingsViewModel()
    {
        _initializing = true;

        if (ThemeService.IsAcrylicMode)
            SelectedTheme = "亚克力";
        else
            SelectedTheme = ThemeService.RootTheme switch
            {
                ElementTheme.Light => "明亮",
                ElementTheme.Dark => "黑暗",
                _ => "跟随系统设置"
            };

        _initializing = false;

        HelpCommand = new RelayCommand(() =>
            NavigationService.Navigate(typeof(Views.MoreInfoPage)));
    }

    /// <summary>
    /// OnSelectedThemeChanged 方法在 SelectedTheme 属性更改时触发，根据新的主题值调用 ThemeService 设置应用程序的主题或启用亚克力模式。
    /// </summary>
    /// <param name="value">新的主题值</param>
    partial void OnSelectedThemeChanged(string value)
    {
        if (_initializing) return;

        switch (value)
        {
            case "黑暗":
                ThemeService.SetTheme(ElementTheme.Dark);
                break;
            case "明亮":
                ThemeService.SetTheme(ElementTheme.Light);
                break;
            case "跟随系统设置":
                ThemeService.SetTheme(ElementTheme.Default);
                break;
            case "亚克力":
                ThemeService.SetAcrylicMode();
                break;
        }
    }
}
