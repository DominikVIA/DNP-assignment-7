using Entities;

namespace ApiContracts.Posts;

public class PostDto
{
    public int Id { get; set; }
    public int authorId { get; set; }
    public string title { get; set; }
    public string body { get; set; } 
    public DateTime dateCreated { get; set; }
    public User author { get; set; }
    public List<Comment> comments { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
}