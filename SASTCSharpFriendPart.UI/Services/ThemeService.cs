using Microsoft.UI.Xaml;
using System.Text.Json;

namespace SASTCSharpFriendPart_UI.Services;

public static class ThemeService
{
    private const string SelectedAppThemeKey = "SelectedAppTheme";
    private const string IsAcrylicModeKey = "IsAcrylicMode";

    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SASTCSharpFriendPart",
        "settings.json"
    );

    public static Window? AppWindow { get; set; }
    public static bool IsAcrylicMode { get; private set; }

    /// <summary>
    /// Callback invoked whenever theme changes so MainWindow can update button colors.
    /// </summary>
    public static Action? OnThemeChanged { get; set; }

    public static ElementTheme RootTheme
    {
        get
        {
            if (Application.Current is App app)
                return app.RequestedTheme switch
                {
                    ApplicationTheme.Light => ElementTheme.Light,
                    ApplicationTheme.Dark => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };
            return ElementTheme.Default;
        }
        set
        {
            if (Application.Current is App app && value != ElementTheme.Default)
            {
                app.RequestedTheme = value switch
                {
                    ElementTheme.Light => ApplicationTheme.Light,
                    ElementTheme.Dark => ApplicationTheme.Dark,
                    _ => ApplicationTheme.Light
                };
            }
            SaveThemeSetting(value.ToString());
        }
    }

    public static void Initialize()
    {
        string? savedTheme = LoadThemeSetting();
        bool isAcrylic = LoadAcrylicModeSetting();
        IsAcrylicMode = isAcrylic;

        if (isAcrylic)
        {
            // Acrylic mode follows system theme; do not override app.RequestedTheme.
            OnThemeChanged?.Invoke();
        }
        else if (!string.IsNullOrEmpty(savedTheme))
        {
            var appTheme = savedTheme switch
            {
                "Light" => ApplicationTheme.Light,
                "Dark" => ApplicationTheme.Dark,
                _ => (ApplicationTheme?)null
            };
            if (appTheme.HasValue && Application.Current is App app)
                app.RequestedTheme = appTheme.Value;
            OnThemeChanged?.Invoke();
        }
    }

    public static bool IsDarkTheme()
    {
        var currentTheme = RootTheme;
        if (currentTheme == ElementTheme.Default)
            return IsSystemDarkTheme();
        return currentTheme == ElementTheme.Dark;
    }

    public static void SetTheme(ElementTheme theme)
    {
        IsAcrylicMode = false;
        SaveAcrylicModeSetting(false);
        RootTheme = theme;

        // Also set the window content theme for immediate visual change
        if (AppWindow?.Content is FrameworkElement fe)
            fe.RequestedTheme = theme;

        OnThemeChanged?.Invoke();
    }

    public static void SetAcrylicMode()
    {
        IsAcrylicMode = true;
        SaveAcrylicModeSetting(true);
        // Acrylic mode follows system theme; leave app.RequestedTheme unchanged.

        if (AppWindow?.Content is FrameworkElement fe)
            fe.RequestedTheme = ElementTheme.Default;

        OnThemeChanged?.Invoke();
    }

    private static bool IsSystemDarkTheme()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                var value = key?.GetValue("AppsUseLightTheme");
                return value is int intValue && intValue == 0;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private static void SaveThemeSetting(string theme)
    {
        SaveSetting(SelectedAppThemeKey, theme);
    }

    private static void SaveAcrylicModeSetting(bool isAcrylic)
    {
        SaveSetting(IsAcrylicModeKey, isAcrylic);
    }

    private static void SaveSetting(string key, object value)
    {
        try
        {
            var settingsDir = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(settingsDir))
                Directory.CreateDirectory(settingsDir!);

            var settings = new Dictionary<string, object>();
            if (File.Exists(SettingsPath))
            {
                var existingJson = File.ReadAllText(SettingsPath);
                var existingSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson);
                if (existingSettings != null)
                    settings = existingSettings;
            }

            settings[key] = value;
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save setting failed: {ex.Message}");
        }
    }

    private static string? LoadThemeSetting()
    {
        var settings = LoadSettings();
        if (settings?.TryGetValue(SelectedAppThemeKey, out var themeValue) == true)
            return themeValue?.ToString();
        return null;
    }

    private static bool LoadAcrylicModeSetting()
    {
        var settings = LoadSettings();
        if (settings?.TryGetValue(IsAcrylicModeKey, out var value) == true)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.True)
                return true;
            if (value is bool boolValue)
                return boolValue;
        }
        return false;
    }

    private static Dictionary<string, object>? LoadSettings()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return null;
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load settings failed: {ex.Message}");
            return null;
        }
    }
}
