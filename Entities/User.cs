using Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // Navigation properties
    public List<Content> Contents { get; set; } = new(); 
    public List<Reaction> Reactions { get; set; } = new(); 
}