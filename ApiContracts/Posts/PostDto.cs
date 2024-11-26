using ApiContracts.Comments;
using ApiContracts.Users;

namespace ApiContracts.Posts;

public class PostDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }  = string.Empty;
    public string Body { get; set; }  = string.Empty;
    public DateTime DateCreated { get; set; }
    public UserDto? Author { get; set; }
    public List<CommentDto> Comments { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}