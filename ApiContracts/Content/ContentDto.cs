namespace ApiContracts.Content;

public class ContentDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; } 
    public string Body { get; set; }
    public DateTime DateCreated { get; set; }
}