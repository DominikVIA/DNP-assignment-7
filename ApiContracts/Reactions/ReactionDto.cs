using Entities;

namespace ApiContracts.Reactions;

public class ReactionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public bool Like { get; set; }
    public DateTime DateCreated { get; set; }
    public User Author { get; set; }
    public Content Content { get; set; }
}