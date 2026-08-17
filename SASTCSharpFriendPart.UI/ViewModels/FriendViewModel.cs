using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SASTCSharpFriendPart_UI.Services;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// FriendViewModel 类是一个视图模型，用于管理和绑定与朋友相关的数据，包括朋友的名称、描述、图片路径以及编辑状态。
/// </summary>
public partial class FriendViewModel : ObservableObject
{
    [ObservableProperty]
    private FriendItemViewModel? selectedFriend;

    [ObservableProperty]
    private string friendName = string.Empty;

    [ObservableProperty]
    private string friendDescription = string.Empty;

    [ObservableProperty]
    private string imagePath = string.Empty;

    [ObservableProperty]
    private bool isEditing;

    private readonly FriendStore _friendStore;

    public ObservableCollection<FriendItemViewModel> Friends => _friendStore.Friends;

    public string ErrorMessage => _friendStore.ErrorMessage;

    public bool IsNotEditing => !IsEditing;

    public Visibility EditButtonVisible =>
        SelectedFriend is not null && !IsEditing
            ? Visibility.Visible : Visibility.Collapsed;

    public Visibility SaveButtonVisible =>
        IsEditing ? Visibility.Visible : Visibility.Collapsed;

    public ImageSource? FriendImage
    {
        get
        {
            if (string.IsNullOrEmpty(ImagePath)) return null;
            try { return new BitmapImage(new Uri($"ms-appx:///{ImagePath}")); }
            catch { return null; }
        }
    }

    public FriendViewModel()
        : this(App.FriendStore)
    {
    }

    public FriendViewModel(FriendStore friendStore)
    {
        _friendStore = friendStore;
        _friendStore.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(FriendStore.ErrorMessage))
            {
                OnPropertyChanged(nameof(ErrorMessage));
            }
        };
        SelectedFriend = Friends.FirstOrDefault();
    }

    /// <summary>
    /// OnSelectedFriendChanged 方法在选择的好友变更时，用其数据初始化编辑草稿。
    /// </summary>
    /// <param name="value">新的朋友名称</param>
    partial void OnSelectedFriendChanged(FriendItemViewModel? value)
    {
        if (value is null) return;

        FriendName = value.Name;
        FriendDescription = value.Description;
        IsEditing = false;
        ImagePath = value.ImgUrl;

        OnPropertyChanged(nameof(EditButtonVisible));
    }

    /// <summary>
    /// OnImagePathChanged 方法在 ImagePath 属性更改时触发，用于通知 UI 更新 FriendImage 属性，以便显示新的朋友图片。
    /// </summary>
    /// <param name="value">新的图片路径</param>
    partial void OnImagePathChanged(string value)
    {
        OnPropertyChanged(nameof(FriendImage));
    }

    /// <summary>
    /// OnIsEditingChanged 方法在 IsEditing 属性更改时触发，用于更新与编辑状态相关的 UI 元素的可见性，包括编辑按钮和保存按钮，同时通知 UI 更新 IsNotEditing 属性。
    /// </summary>
    /// <param name="value">新的编辑状态</param>
    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotEditing));
        OnPropertyChanged(nameof(EditButtonVisible));
        OnPropertyChanged(nameof(SaveButtonVisible));
    }

    /// <summary>
    /// Edit 方法用于将当前视图模型的编辑状态设置为 true，以便用户可以编辑朋友的详细信息。
    /// </summary>
    [RelayCommand]
    private void Edit()
    {
        IsEditing = true;
    }

    /// <summary>
    /// Save 方法用于保存当前编辑的朋友信息，将更改应用到 _friends 列表中，并将更新后的列表写入 JSON 文件，同时更新 FriendNames 集合和 SelectedFriendName 属性，以反映更改后的朋友名称。
    /// </summary>
    [RelayCommand]
    private void Save()
    {
        if (SelectedFriend is null)
        {
            return;
        }

        SelectedFriend.Name = FriendName;
        SelectedFriend.Description = FriendDescription;
        ImagePath = SelectedFriend.ImgUrl;

        if (_friendStore.Save())
        {
            IsEditing = false;
        }
    }
}
