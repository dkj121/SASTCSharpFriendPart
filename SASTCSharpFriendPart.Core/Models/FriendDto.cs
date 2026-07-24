namespace SASTCSharpFriendPart.Core.Models;

/// <summary>
/// FriendDto类用于表示好友信息的传输对象，包含好友的基本信息，如姓名、邮箱、描述和头像URL，与远程系统进行数据交换。
/// </summary>
public record class FriendDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImgUrl { get; set; } = string.Empty;
}
