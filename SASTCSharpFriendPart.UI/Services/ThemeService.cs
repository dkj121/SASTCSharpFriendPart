using Microsoft.UI.Xaml;

using System.Text.Json;

namespace SASTCSharpFriendPart_UI.Services;

/// <summary>
/// ThemeService 提供了一个静态类，用于管理应用程序的主题设置，包括浅色主题、深色主题和亚克力模式。
/// </summary>
public static class ThemeService
{
    /// <summary>
    /// SelectedAppThemeKey 是用于存储和检索用户选择的应用程序主题的键。
    /// </summary>
    private const string SelectedAppThemeKey = "SelectedAppTheme";

    /// <summary>
    /// IsAcrylicModeKey 是用于存储和检索用户是否启用亚克力模式的键。
    /// </summary>
    private const string IsAcrylicModeKey = "IsAcrylicMode";

    /// <summary>
    /// SettingsPath 是用于存储应用程序设置的 JSON 文件的路径，包括主题设置和亚克力模式设置。
    /// </summary>
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SASTCSharpFriendPart",
        "settings.json"
    );

    /// <summary>
    /// AppWindow 属性用于获取或设置应用程序的主窗口，以便在主题更改时更新窗口内容的主题。
    /// </summary>
    public static Window? AppWindow { get; set; }

    /// <summary>
    /// IsAcrylicMode 属性用于指示当前应用程序是否处于亚克力模式。
    /// </summary>
    public static bool IsAcrylicMode { get; private set; }

    /// <summary>
    /// Callback invoked whenever theme changes so MainWindow can update button colors.
    /// </summary>
    public static Action? OnThemeChanged { get; set; }

    /// <summary>
    /// RootTheme 属性用于获取或设置应用程序的根主题（浅色、深色或默认），并在设置更改时保存到配置文件中。
    /// </summary>
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

    /// <summary>
    /// Initialize 方法用于初始化主题设置，从配置文件中加载用户的主题选择和亚克力模式设置，并应用到应用程序中。
    /// </summary>
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

    /// <summary>
    /// IsDarkTheme 方法用于检查当前应用程序是否处于深色主题模式，如果根主题为默认，则根据系统主题判断。
    /// </summary>
    /// <returns>返回一个布尔值，指示应用程序是否处于深色主题模式</returns>
    public static bool IsDarkTheme()
    {
        var currentTheme = RootTheme;
        if (currentTheme == ElementTheme.Default)
            return IsSystemDarkTheme();
        return currentTheme == ElementTheme.Dark;
    }

    /// <summary>
    /// SetTheme 方法用于设置应用程序的主题为指定的 ElementTheme，并禁用亚克力模式，同时保存设置到配置文件中。
    /// </summary>
    /// <param name="theme">主题</param>
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

    /// <summary>
    /// SetAcrylicMode 方法用于启用亚克力模式，同时保存设置到配置文件中。
    /// </summary>
    public static void SetAcrylicMode()
    {
        IsAcrylicMode = true;
        SaveAcrylicModeSetting(true);
        // Acrylic mode follows system theme; leave app.RequestedTheme unchanged.

        if (AppWindow?.Content is FrameworkElement fe)
            fe.RequestedTheme = ElementTheme.Default;

        OnThemeChanged?.Invoke();
    }

    /// <summary>
    /// IsSystemDarkTheme 方法用于检查系统当前是否处于深色主题模式，通过读取 Windows 注册表中的主题设置来判断。
    /// </summary>
    /// <returns>返回一个布尔值，指示系统是否处于深色主题模式</returns>
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

    /// <summary>
    /// SaveThemeSetting 方法用于将用户选择的应用程序主题保存到配置文件中，以便在应用程序重新启动时恢复该主题设置。
    /// </summary>
    /// <param name="theme">主题</param>
    private static void SaveThemeSetting(string theme)
    {
        SaveSetting(SelectedAppThemeKey, theme);
    }

    /// <summary>
    /// SaveAcrylicModeSetting 方法用于将亚克力模式的设置保存到配置文件中。
    /// </summary>
    /// <param name="isAcrylic">指示是否启用亚克力模式的布尔值</param>
    private static void SaveAcrylicModeSetting(bool isAcrylic)
    {
        SaveSetting(IsAcrylicModeKey, isAcrylic);
    }

    /// <summary>
    /// SaveSetting 方法用于将指定的键值对保存到配置文件中，如果配置文件不存在，则创建该文件。
    /// </summary>
    /// <param name="key">储存位置</param>
    /// <param name="value">储存值</param>
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

    /// <summary>
    /// LoadThemeSetting 方法用于从配置文件中加载用户选择的应用程序主题。
    /// </summary>
    /// <returns>返回加载的主题名称，如果未找到则返回 null</returns>
    private static string? LoadThemeSetting()
    {
        var settings = LoadSettings();
        if (settings?.TryGetValue(SelectedAppThemeKey, out var themeValue) == true)
            return themeValue?.ToString();
        return null;
    }

    /// <summary>
    /// LoadAcrylicModeSetting 方法用于从配置文件中加载亚克力模式的设置。
    /// </summary>
    /// <returns>返回一个布尔值，指示是否启用了亚克力模式</returns>
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

    /// <summary>
    /// LoadSettings 方法用于从配置文件中加载所有的设置，并返回一个包含键值对的字典，如果配置文件不存在或读取失败，则返回 null。
    /// </summary>
    /// <returns></returns>
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
