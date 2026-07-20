using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SASTCSharpFriendPart_UI.ViewModels;

namespace SASTCSharpFriendPart_UI.Views;

public sealed partial class FriendPage : Page
{
    public FriendViewModel ViewModel { get; } = new();

    public FriendPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EntranceAnimation.Begin();
    }

    /// <summary>
    /// Edit button visible when a friend is selected and not currently editing.
    /// </summary>
    public static Visibility EditButtonVisible(string selectedName, bool isEditing)
    {
        if (string.IsNullOrEmpty(selectedName) || isEditing)
            return Visibility.Collapsed;
        return Visibility.Visible;
    }

    /// <summary>
    /// Invert bool — used for IsReadOnly (read-only when NOT editing).
    /// </summary>
    public static bool InvertBool(bool value) => !value;

    /// <summary>
    /// Create a BitmapImage from a file path string for {x:Bind} (avoids string→Uri conversion issues).
    /// </summary>
    public static ImageSource? GetImageSource(string path)
    {
        if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
            return null;
        return new BitmapImage(new Uri(path));
    }
}
