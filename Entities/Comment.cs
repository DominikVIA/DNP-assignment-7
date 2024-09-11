namespace Entities;

public class Comment(
    int authorId,
    int respondingToId,
    string body,
    DateTime dateCreated)
    : Content(authorId, body, dateCreated)
{
    public int RespondingToId { get; } = respondingToId;
}