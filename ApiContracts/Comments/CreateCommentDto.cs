namespace ApiContracts.Comments;

public class CreateCommentDto
{
    public int AuthorId { get; set; }
    public int RespondingToId { get; set; }
    public string Body { get; set; }
}