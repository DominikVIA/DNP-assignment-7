using ApiContracts.Comments;
using Entities;

namespace ApiContracts.Posts;

public class PostDto
{
    public int Id { get; set; }
    public int authorId { get; set; }
    public string title { get; set; }  = string.Empty;
    public string body { get; set; }  = string.Empty;
    public DateTime dateCreated { get; set; }
    public User author { get; set; }
    public List<CommentDto> comments { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
}