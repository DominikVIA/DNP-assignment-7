namespace Entities;

public class Post(
    int authorId,
    string title,
    string body,
    DateTime dateCreated)
    : Content(authorId, body, dateCreated)
{
    public string Title { get; set; } = title;
}