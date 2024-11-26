using ApiContracts.Comments;
using ApiContracts.Posts;

namespace ApiContracts.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<PostDto> Posts { get; set; }
    public List<CommentDto> Comments { get; set; }
}

