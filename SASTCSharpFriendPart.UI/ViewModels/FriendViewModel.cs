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

    /// <summary>Inverse of IsEditing — used directly by {x:Bind} for IsReadOnly.</summary>
    public bool IsNotEditing => !IsEditing;

    /// <summary>Edit button visible when a friend is selected and NOT editing.</summary>
    public Visibility EditButtonVisible =>
        !string.IsNullOrEmpty(SelectedFriendName) && !IsEditing
            ? Visibility.Visible : Visibility.Collapsed;

    /// <summary>Image source derived from ImagePath with path-traversal guard.</summary>
    public ImageSource? FriendImage
    {
        get
        {
            if (string.IsNullOrEmpty(ImagePath)) return null;
            try
            {
                string fullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, ImagePath));
                string baseDir = Path.GetFullPath(AppContext.BaseDirectory);
                if (!fullPath.StartsWith(baseDir + Path.DirectorySeparatorChar) || !File.Exists(fullPath))
                    return null;
                return new BitmapImage(new Uri(fullPath));
            }
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

        string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, friend.ImgUrl));
        string baseDir = Path.GetFullPath(AppContext.BaseDirectory);
        ImagePath = (path.StartsWith(baseDir + Path.DirectorySeparatorChar) && File.Exists(path))
            ? path : string.Empty;

        OnPropertyChanged(nameof(EditButtonVisible));
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotEditing));
        OnPropertyChanged(nameof(EditButtonVisible));
    }

    [RelayCommand]
    private void Edit()
    {
        IsEditing = true;
    }
}
