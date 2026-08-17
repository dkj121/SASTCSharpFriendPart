using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SASTCSharpFriendPart_UI.Services;
using SASTCSharpFriendPart_UI.Views;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// MainWindowViewModel 类是一个视图模型，用于管理主窗口的导航操作，包括页面导航和返回操作。
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    /// <summary>
    /// NavigateCommand 属性是一个命令，用于处理页面导航操作，根据传入的页面名称导航到相应的页面类型，并通过 NavigationService 进行导航。
    /// </summary>
    public IRelayCommand<string> NavigateCommand { get; }

    /// <summary>
    /// MainWindowViewModel 构造函数用于初始化 MainWindowViewModel 实例，设置 NavigateCommand 命令的执行逻辑，根据页面名称导航到对应的页面类型。
    /// </summary>
    public MainWindowViewModel()
    {
        NavigateCommand = new RelayCommand<string>(pageName =>
        {
            var pageType = pageName switch
            {
                "WelcomePage" => typeof(WelcomePage),
                "FriendPage" => typeof(FriendPage),
                "ChatPage" => typeof(ChatPage),
                "PersonalPage" => typeof(PersonalPage),
                "SettingsPage" => typeof(SettingsPage),
                _ => null
            };
            if (pageType != null)
                NavigationService.Navigate(pageType);
        });
    }
}
