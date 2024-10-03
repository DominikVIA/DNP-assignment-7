namespace ApiContracts.Reactions;

public class CreateReactionDto
{
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public bool Like { get; set; }
}