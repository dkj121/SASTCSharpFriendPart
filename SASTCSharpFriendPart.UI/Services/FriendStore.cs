using System.Collections.ObjectModel;
using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;

using SASTCSharpFriendPart_UI.ViewModels;

using SASTCSharpFriendPart.Core.Models;

namespace SASTCSharpFriendPart_UI.Services;

/// <summary>
/// Owns the application's shared friend collection and its per-user JSON persistence.
/// </summary>
public partial class FriendStore : ObservableObject
{
    private const string DataFileName = "data.json";

    public FriendStore()
    {
        Load();
    }

    public ObservableCollection<FriendItemViewModel> Friends { get; } = new();

    public string DataPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SASTCSharpFriendPart",
        DataFileName);

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public bool Save()
    {
        try
        {
            var directory = Path.GetDirectoryName(DataPath)!;
            Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(
                Friends.Select(friend => friend.ToDto()),
                new JsonSerializerOptions { WriteIndented = true });
            var temporaryPath = Path.Combine(directory, $"{DataFileName}.{Guid.NewGuid():N}.tmp");

            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, DataPath, true);
            ErrorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unable to save friend data: {ex.Message}";
            return false;
        }
    }

    private void Load()
    {
        try
        {
            var directory = Path.GetDirectoryName(DataPath)!;
            Directory.CreateDirectory(directory);

            if (!File.Exists(DataPath))
            {
                var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", DataFileName);
                if (!File.Exists(seedPath))
                {
                    ErrorMessage = "The initial friend data file could not be found.";
                    return;
                }

                File.Copy(seedPath, DataPath);
            }

            var json = File.ReadAllText(DataPath);
            var friends = JsonSerializer.Deserialize<List<FriendDto>>(json) ?? new();

            var nextId = friends.Count == 0 ? 1 : friends.Max(friend => friend.Id) + 1;
            foreach (var friend in friends)
            {
                if (friend.Id == 0)
                {
                    friend.Id = nextId++;
                }

                Friends.Add(new FriendItemViewModel(friend));
            }

            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unable to load friend data: {ex.Message}";
        }
    }
}
