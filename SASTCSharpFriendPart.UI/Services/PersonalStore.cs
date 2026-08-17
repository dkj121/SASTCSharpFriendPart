using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SASTCSharpFriendPart_UI.Services;

/// <summary>
/// Owns the current user's editable profile and persists it locally.
/// </summary>
public partial class PersonalStore : ObservableObject
{
    private const string DataFileName = "personal.json";

    public PersonalStore()
    {
        Load();
    }

    public string DataPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SASTCSharpFriendPart",
        DataFileName);

    [ObservableProperty]
    private string name = "鸽子";

    [ObservableProperty]
    private string description = "简介被鸽了";

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public bool Save()
    {
        try
        {
            var directory = Path.GetDirectoryName(DataPath)!;
            Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(
                new PersonalProfile(Name, Description),
                new JsonSerializerOptions { WriteIndented = true });
            var temporaryPath = Path.Combine(directory, $"{DataFileName}.{Guid.NewGuid():N}.tmp");

            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, DataPath, true);
            ErrorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"无法保存个人信息：{ex.Message}";
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
                Save();
                return;
            }

            var profile = JsonSerializer.Deserialize<PersonalProfile>(File.ReadAllText(DataPath));
            if (profile is not null)
            {
                Name = profile.Name ?? string.Empty;
                Description = profile.Description ?? string.Empty;
            }

            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"无法加载个人信息：{ex.Message}";
        }
    }

    private sealed record PersonalProfile(string Name, string Description);
}
