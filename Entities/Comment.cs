namespace Entities;

public class Comment(
    int authorId,
    int respondingToId,
    string body,
    List<int> likes,
    List<int> dislikes,
    DateTime dateCreated)
    : Content(authorId, body, likes, dislikes, dateCreated)
{
    public int RespondingToId { get; } = respondingToId;
}