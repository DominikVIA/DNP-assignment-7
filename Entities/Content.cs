namespace Entities;

public class Content
{
    public int Id { get; set; }
    public int AuthorId { get; set; } 
    public string Body { get; set; }
    public DateTime DateCreated { get; set; }

    // Navigation properties
    public User Author { get; set; } = null!; 
    public List<Reaction> Reactions { get; set; } = []; 
    public List<Comment> Comments { get; set; } = [];
    
}
