using Microsoft.UI.Xaml.Controls;

namespace SASTCSharpFriendPart_UI.Services;

public static class NavigationService
{
    public static Frame? ContentFrame { get; set; }

    public static void Navigate(Type pageType, object? parameter = null)
    {
        if (ContentFrame == null) return;

        try
        {
            ContentFrame.Navigate(pageType, parameter);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
        }
    }

    public static void GoBack()
    {
        if (ContentFrame is { CanGoBack: true })
            ContentFrame.GoBack();
    }

    public static bool CanGoBack => ContentFrame?.CanGoBack ?? false;
}
