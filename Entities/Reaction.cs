namespace Entities;

public class Reaction (
    int userId, 
    int contentId, 
    bool like, 
    bool dislike, 
    DateTime dateCreated)
{
    public int Id { get; set; } = -1;
    public int UserId { get; set; } = userId;
    public int ContentId { get; set; } = contentId;
    public bool Like { get; set; } = like;
    public bool Dislike { get; set; } = dislike;
    public DateTime DateCreated { get; set; } = dateCreated;
}