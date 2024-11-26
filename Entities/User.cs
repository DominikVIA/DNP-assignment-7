using Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // Navigation properties
    // public List<Content> Contents { get; set; } = new(); 
    public List<Post> Posts { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    // public List<Reaction> Reactions { get; set; } = new(); 
}