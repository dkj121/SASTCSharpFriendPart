using CommunityToolkit.Mvvm.ComponentModel;

using SASTCSharpFriendPart.Core.Models;

namespace SASTCSharpFriendPart_UI.ViewModels;

/// <summary>
/// A bindable representation of a friend while retaining the Core DTO as the persisted contract.
/// </summary>
public partial class FriendItemViewModel : ObservableObject
{
    private readonly FriendDto _friend;

    public FriendItemViewModel(FriendDto friend)
    {
        _friend = friend;
        Id = friend.Id;
        name = friend.Name;
        email = friend.Email;
        description = friend.Description;
        imgUrl = friend.ImgUrl;
    }

    public int Id { get; }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private string imgUrl;

    partial void OnNameChanged(string value) => _friend.Name = value;

    partial void OnEmailChanged(string value) => _friend.Email = value;

    partial void OnDescriptionChanged(string value) => _friend.Description = value;

    partial void OnImgUrlChanged(string value) => _friend.ImgUrl = value;

    public FriendDto ToDto() => _friend;
}
