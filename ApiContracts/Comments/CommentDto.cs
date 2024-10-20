using Entities;

namespace ApiContracts.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public int AuthorId { get; set;  }
    public string Body { get; set; }
    public DateTime DateCreated { get; set; }
    public int RespondingToId { get; set; }
    public User Author { get; set; }
    public Post RespondingTo { get; set; }
}