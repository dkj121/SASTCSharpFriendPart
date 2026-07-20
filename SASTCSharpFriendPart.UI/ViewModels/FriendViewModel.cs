using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SASTCSharpFriendPart.Core.Models;

using System.Collections.ObjectModel;
using System.Text.Json;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class FriendViewModel : ObservableObject
{
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

    public FriendViewModel()
    {
        LoadFriends();
    }

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

    partial void OnImagePathChanged(string value)
    {
        OnPropertyChanged(nameof(FriendImage));
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotEditing));
        OnPropertyChanged(nameof(EditButtonVisible));
        OnPropertyChanged(nameof(SaveButtonVisible));
    }

    [RelayCommand]
    private void Edit()
    {
        IsEditing = true;
    }

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
