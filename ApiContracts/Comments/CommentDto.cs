using ApiContracts.Content;
using ApiContracts.Users;

namespace ApiContracts.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public int AuthorId { get; set;  }
    public string Body { get; set; }
    public DateTime DateCreated { get; set; }
    public int RespondingToId { get; set; }
    public UserDto? Author { get; set; }
    public ContentDto? RespondingTo { get; set; }
    public List<CommentDto>? Comments { get; set; }
}