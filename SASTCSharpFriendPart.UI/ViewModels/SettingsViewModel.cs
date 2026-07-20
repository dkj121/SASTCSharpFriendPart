using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SASTCSharpFriendPart_UI.Services;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private bool _initializing;

    [ObservableProperty]
    private ObservableCollection<string> themes = new() { "明亮", "黑暗", "跟随系统设置", "亚克力" };

    [ObservableProperty]
    private string selectedTheme;

    public IRelayCommand HelpCommand { get; }

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
