namespace Entities;

public abstract class Content(
    int authorId,
    string body,
    DateTime dateCreated)
{
    public int Id { get; set; } = -1;
    public int AuthorId { get; set;  } = authorId;
    public string Body { get; set; } = body;
    public List<int> Reactions { get; set;  } = new();
    // public List<int> Dislikes { get; set;  } = new();
    public DateTime DateCreated { get; set; } = dateCreated;

    public Task addReaction(int reactionId)
    {
        Reactions.Add(reactionId);
        return Task.CompletedTask;
    }
}