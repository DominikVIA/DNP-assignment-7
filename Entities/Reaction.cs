namespace Entities;

public class Reaction (
    int userId, 
    int contentId, 
    bool like, 
    DateTime dateCreated)
{
    public int Id { get; set; } = -1;
    public int UserId { get; set; } = userId;
    public int ContentId { get; set; } = contentId;
    public bool Like { get; set; } = like;
    public DateTime DateCreated { get; set; } = dateCreated;
}