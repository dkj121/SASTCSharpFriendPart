using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.VisualBasic;

namespace SASTCSharpFriendPart.Core.Models;

/// <summary>
/// Friend类用于表示具体好友。
/// </summary>
public class Friend
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImgUrl { get; set; } = string.Empty;
    public string GifUrl { get; set; } = string.Empty;
    public (int, int) Place { get; set; } = (0, 0);
    public Friend(string name, string email, string description, string imgUrl)
    {
        this.Name = name;
        this.Email = email;
        this.Description = description;
        this.ImgUrl = imgUrl;
    }
    public Friend(FriendDto friendDto)
    {
        this.Name = friendDto.Name;
        this.Email = friendDto.Email;
        this.Description = friendDto.Description;
        this.ImgUrl = friendDto.ImgUrl;
    }
}
