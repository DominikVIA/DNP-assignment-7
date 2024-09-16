using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class UpdateCommentView (ICommentRepository commentRepo)
{
    public Task DeleteComment(int id)
    {
        commentRepo.DeleteAsync(id);
        return Task.CompletedTask;
    }
    
    public Task UpdateComment(int id, int respondingToId, string body)
    {
        Comment commentToEdit;
        try
        {
            commentToEdit = commentRepo.GetSingleAsync(id).Result;
        }
        catch(InvalidOperationException e)       {
            throw new ArgumentException(e.Message);
        }

        commentToEdit.Body = body;
        commentRepo.UpdateAsync(commentToEdit);
        return Task.CompletedTask;
    }
}