namespace Entities;

public class Comment : Content
{
    public int RespondingToId { get; set; }

    // Navigation property 
    public Content RespondingTo { get; set; } = null!;
}
