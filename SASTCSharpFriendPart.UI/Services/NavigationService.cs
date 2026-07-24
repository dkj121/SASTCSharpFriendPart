using Microsoft.UI.Xaml.Controls;

namespace SASTCSharpFriendPart_UI.Services;

/// <summary>
/// NavigationService 提供了一个静态类，用于管理应用程序的导航操作，包括页面导航和返回操作。
/// </summary>
public static class NavigationService
{
    /// <summary>
    /// ContentFrame 属性用于获取或设置应用程序的主内容框架，用于承载导航的页面。
    /// </summary>
    public static Frame? ContentFrame { get; set; }

    /// <summary>
    /// Navigate 方法用于导航到指定的页面类型，并可选择传递参数。
    /// </summary>
    /// <param name="pageType">要导航到的页面类型。</param>
    /// <param name="parameter">传递给目标页面的参数。</param>
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

    /// <summary>
    /// GoBack 方法用于导航返回上一个页面。
    /// </summary>
    public static void GoBack()
    {
        if (ContentFrame is { CanGoBack: true })
            ContentFrame.GoBack();
    }

    /// <summary>
    /// CanGoBack 属性用于检查是否可以导航返回上一个页面。
    /// </summary>
    public static bool CanGoBack => ContentFrame?.CanGoBack ?? false;
}
