namespace Entities;

public abstract class Content(
    int authorId,
    string body,
    List<int> likes,
    List<int> dislikes,
    DateTime dateCreated)
{
    public int Id { get; set; } = -1;
    public int AuthorId { get; set;  } = authorId;
    public string Body { get; set; } = body;
    public List<int> Likes { get; set;  } = likes;
    public List<int> Dislikes { get; set;  } = dislikes;
    public DateTime DateCreated { get; set; } = dateCreated;
}