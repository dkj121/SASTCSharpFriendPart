using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SASTCSharpFriendPart.Core.Models;

using System.Collections.ObjectModel;
using System.Text.Json;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// FriendViewModel 类是一个视图模型，用于管理和绑定与朋友相关的数据，包括朋友的名称、描述、图片路径以及编辑状态。
/// </summary>
public partial class FriendViewModel : ObservableObject
{
    /// <summary>
    /// _friends 字段是一个私有的 List，用于存储所有朋友的详细信息，包括名称、描述和图片路径。
    /// </summary>
    private List<FriendDto> _friends = new();

    [ObservableProperty]
    private ObservableCollection<string> friendNames = new();

    [ObservableProperty]
    private string selectedFriendName = string.Empty;

    [ObservableProperty]
    private string friendName = string.Empty;

    [ObservableProperty]
    private string friendDescription = string.Empty;

    [ObservableProperty]
    private string imagePath = string.Empty;

    [ObservableProperty]
    private bool isEditing;

    public bool IsNotEditing => !IsEditing;

    public Visibility EditButtonVisible =>
        !string.IsNullOrEmpty(SelectedFriendName) && !IsEditing
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

    /// <summary>
    /// FriendViewModel 构造函数用于初始化 FriendViewModel 实例，并加载朋友数据，从 JSON 文件中读取朋友信息并填充到 ViewModel 中。
    /// </summary>
    public FriendViewModel()
    {
        LoadFriends();
    }

    /// <summary>
    /// LoadFriends 方法用于从指定的 JSON 文件中加载朋友数据，并将其存储在 _friends 列表中，同时更新 FriendNames 集合以供 UI 绑定使用。
    /// </summary>
    private void LoadFriends()
    {
        try
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "data.json");
            if (!File.Exists(jsonPath)) return;

            string jsonData = File.ReadAllText(jsonPath);
            _friends = JsonSerializer.Deserialize<List<FriendDto>>(jsonData) ?? new();

            FriendNames.Clear();
            foreach (var f in _friends) FriendNames.Add(f.Name);

            if (FriendNames.Count > 0)
                SelectedFriendName = FriendNames[0];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading friends: {ex.Message}");
        }
    }

    /// <summary>
    /// OnSelectedFriendNameChanged 方法在 SelectedFriendName 属性更改时触发，用于更新当前选中朋友的详细信息，包括名称、描述和图片路径，同时设置编辑状态为 false。
    /// </summary>
    /// <param name="value">新的朋友名称</param>
    partial void OnSelectedFriendNameChanged(string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        var friend = _friends.FirstOrDefault(f => f.Name == value);
        if (friend == null) return;

        FriendName = friend.Name;
        FriendDescription = friend.Description;
        IsEditing = false;
        var fullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, friend.ImgUrl));
        var baseDir = Path.GetFullPath(AppContext.BaseDirectory);
        ImagePath = fullPath.StartsWith(baseDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            ? friend.ImgUrl : string.Empty;

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
        var friend = _friends.FirstOrDefault(f => f.Name == SelectedFriendName);
        if (friend != null)
        {
            friend.Name = FriendName;
            friend.Description = FriendDescription;
        }

        WriteFriendsToJson();

        if (FriendName != SelectedFriendName)
        {
            var idx = FriendNames.IndexOf(SelectedFriendName);
            FriendNames[idx] = FriendName;
            SelectedFriendName = FriendName;
        }

        IsEditing = false;
    }

    /// <summary>
    /// WriteFriendsToJson 方法用于将当前 _friends 列表中的朋友信息序列化为 JSON 格式，并写入到指定的 JSON 文件中，以便在应用程序重新启动时能够加载最新的朋友数据。
    /// </summary>
    private void WriteFriendsToJson()
    {
        try
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "data.json");
            var json = JsonSerializer.Serialize(_friends, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving friends: {ex.Message}");
        }
    }
}
