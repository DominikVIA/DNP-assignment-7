namespace Entities;

public class Reaction
{
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public bool Like { get; set; }
    public DateTime DateCreated { get; set; }

    // Navigation properties
    public User User { get; set; } = null!; 
    public Content Content { get; set; } = null!; 
}
