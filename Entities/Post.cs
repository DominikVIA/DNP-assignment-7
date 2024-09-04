namespace Entities;

public class Post(
    int authorId,
    string title,
    string body,
    List<int> likes,
    List<int> dislikes,
    DateTime dateCreated)
    : Content(authorId, body, likes, dislikes, dateCreated)
{
    public string Title { get; } = title;
}