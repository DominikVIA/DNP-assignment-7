using ApiContracts.Content;
using ApiContracts.Users;

namespace ApiContracts.Reactions;

public class ReactionDto
{
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public bool Like { get; set; }
    public DateTime DateCreated { get; set; }
    public UserDto? Author { get; set; }
    public ContentDto? Content { get; set; }
}