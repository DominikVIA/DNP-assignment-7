namespace ApiContracts.Posts;

public class CreatePostDto
{
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}