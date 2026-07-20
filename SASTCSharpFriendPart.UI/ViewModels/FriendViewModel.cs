using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public FriendViewModel()
    {
        LoadFriends();
    }

    private void LoadFriends()
    {
        try
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "data.json");
            if (!File.Exists(jsonPath))
                return;

            string jsonData = File.ReadAllText(jsonPath);
            _friends = JsonSerializer.Deserialize<List<FriendDto>>(jsonData) ?? new();

            FriendNames.Clear();
            foreach (var friend in _friends)
                FriendNames.Add(friend.Name);

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

        string imagePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, friend.ImgUrl));
        string baseDir = Path.GetFullPath(AppContext.BaseDirectory);
        ImagePath = (imagePath.StartsWith(baseDir + Path.DirectorySeparatorChar) && File.Exists(imagePath))
            ? imagePath : string.Empty;
    }

    [RelayCommand]
    private void Edit()
    {
        IsEditing = true;
    }
}
