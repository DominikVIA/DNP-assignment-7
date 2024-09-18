namespace Entities;

public abstract class Content(
    int authorId,
    string body,
    DateTime dateCreated)
{
    public int Id { get; set; } = -1;
    public int AuthorId { get; set;  } = authorId;
    public string Body { get; set; } = body;
    public DateTime DateCreated { get; set; } = dateCreated;
}