
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView (ICommentRepository commentRepo)
{
    public async Task<Comment> CreateComment(int authorId, int respondingToId, string body)
    {
        Comment comment = await commentRepo.AddAsync(new Comment(authorId, respondingToId, body, DateTime.Now));
        return comment;
    }

}