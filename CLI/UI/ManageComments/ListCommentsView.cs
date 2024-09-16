using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView (ICommentRepository commentRepo)
{
    public IQueryable<Comment> GetAllComments()
    {
        return commentRepo.GetMany();
    }
    
    public Task<Comment> GetComment (int id)
    {
        return commentRepo.GetSingleAsync(id);
    }
}