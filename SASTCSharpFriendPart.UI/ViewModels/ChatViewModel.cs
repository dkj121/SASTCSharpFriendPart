using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SASTCSharpFriendPart_UI.Services;

namespace SASTCSharpFriendPart_UI.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly FriendStore _friendStore;

    private ChatContactViewModel? selectedContact;

    public ObservableCollection<ChatContactViewModel> Contacts { get; } = new();

    public ChatContactViewModel? SelectedContact
    {
        get => selectedContact;
        set => SetProperty(ref selectedContact, value);
    }

    public ChatViewModel()
        : this(App.FriendStore)
    {
    }

    public ChatViewModel(FriendStore friendStore)
    {
        _friendStore = friendStore;

        foreach (var friend in _friendStore.Friends)
        {
            Contacts.Add(new ChatContactViewModel(friend));
        }

        SelectedContact = Contacts.FirstOrDefault();
    }
}

public class ChatContactViewModel
{
    public ChatContactViewModel(FriendItemViewModel friend)
    {
        Name = friend.Name;
        Description = friend.Description;
        Avatar = CreateAvatar(friend.ImgUrl);
        Messages = CreateMessages(friend.Id, Name, Description);
        PreviewText = Messages.Last().Content;
        PreviewTime = Messages.Last().Time;
    }

    public string Name { get; }

    public string Description { get; }

    public ImageSource? Avatar { get; }

    public IReadOnlyList<ChatMessageViewModel> Messages { get; }

    public string PreviewText { get; }

    public string PreviewTime { get; }

    private static IReadOnlyList<ChatMessageViewModel> CreateMessages(int id, string name, string description)
    {
        return id switch
        {
            1 =>
            [
                new("12:18", "Hey，森林边新开了条小路，要不要一起去看看？", false),
                new("12:20", "好呀，我正想找个地方散散步", true),
                new("12:22", "好哦，我带了下午茶，在老橡树旁等你", false),
                new("12:24", "一言为定，马上来！", true)
            ],
            2 =>
            [
                new("11:46", "傍晚的沙丘风小很多，今天很适合出去走走", false),
                new("11:48", "确实，比白天凉快多了", true),
                new("11:50", "我这儿有个背风的小沙坡，抬头还能看到特别亮的星星", false),
                new("11:52", "那我带点水过去，星空下见！", true),
                new("11:53", "Hirari do!", false)
            ],
            3 =>
            [
                new("09:15", "湖边的芦苇开花了，水面上还有好多小鱼", false),
                new("09:18", "听上去不错，今天适合拍照吗？", true),
                new("09:20", "当然，晨光撒在水面上，特别漂亮", false),
                new("09:23", "我这就带相机过去", true)
            ],
            4 =>
            [
                new("14:03", "雨后的石头暖得很快，林子里的气味也变得很新鲜", false),
                new("14:05", "听起来是散步的好时候，路会不会有点滑？", true),
                new("14:07", "沿着南边那条干燥的小径走就没问题，我已经探过路了", false),
                new("14:10", "收到，我会慢慢走过去", true)
            ],
            _ => CreateFallbackMessages(name, description)
        };
    }

    private static IReadOnlyList<ChatMessageViewModel> CreateFallbackMessages(string name, string description)
    {
        var introduction = string.IsNullOrWhiteSpace(description)
            ? $"你好，我是 {name}！"
            : $"你好呀，我是 {name}。{description}，很高兴认识你！";

        return
        [
            new("刚刚", introduction, false),
            new("刚刚", $"你好，{name}！以后请多关照。", true),
            new("刚刚", "我最近发现了一个安静的地方，有空一起去看看吗？", false),
            new("刚刚", "当然可以，等你方便的时候告诉我。", true)
        ];
    }

    private static ImageSource? CreateAvatar(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return null;
        }

        try
        {
            return new BitmapImage(new Uri($"ms-appx:///{imagePath}"));
        }
        catch
        {
            return null;
        }
    }
}

public class ChatMessageViewModel
{
    public ChatMessageViewModel(string time, string content, bool isMine)
    {
        Time = time;
        Content = content;
        IsMine = isMine;
    }

    public string Time { get; }

    public string Content { get; }

    public bool IsMine { get; }

    public Visibility MineVisibility => IsMine ? Visibility.Visible : Visibility.Collapsed;

    public Visibility FriendVisibility => IsMine ? Visibility.Collapsed : Visibility.Visible;
}
