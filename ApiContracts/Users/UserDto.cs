using Entities;

namespace ApiContracts.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Post> Posts { get; set; }
    public List<Comment> Comments { get; set; }
}

