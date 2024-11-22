namespace Entities;

public class Comment : Content
{
    public int RespondingToId { get; set; }

    // Navigation property 
    public Content RespondingTo { get; set; } = null!;

    // public Comment(int authorId, int respondingToId, string body, DateTime dateCreated)
    //     : base(authorId, body, dateCreated)
    // {
    //     RespondingToId = respondingToId;
    // }
   // private Post(){}

}
