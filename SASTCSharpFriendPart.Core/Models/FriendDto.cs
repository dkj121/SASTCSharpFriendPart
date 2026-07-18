namespace SASTCSharpFriendPart.Core.Models
{
    public record class FriendDto
    {
        public string Name { get; set; } = string.Empty;
        public uint Age { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
    }
}