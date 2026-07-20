using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SASTCSharpFriendPart_UI.Services;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public IRelayCommand<string> NavigateCommand { get; }

    public MainWindowViewModel()
    {
        NavigateCommand = new RelayCommand<string>(pageName =>
        {
            var pageType = pageName switch
            {
                "WelcomePage" => typeof(Views.WelcomePage),
                "FriendPage" => typeof(Views.FriendPage),
                "SettingsPage" => typeof(Views.SettingsPage),
                _ => null
            };
            if (pageType != null)
                NavigationService.Navigate(pageType);
        });
    }
}
