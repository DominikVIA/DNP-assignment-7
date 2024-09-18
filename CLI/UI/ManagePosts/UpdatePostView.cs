using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class UpdatePostView (IPostRepository postRepo)
{
    public async Task DeletePost(int id)
    {
        await postRepo.DeleteAsync(id);
    }
    
    public Task UpdatePost(int id, string title, string body)
    {
        Post postToEdit;
        try
        {
            postToEdit = postRepo.GetSingleAsync(id).Result;
        }
        catch(InvalidOperationException e)       {
            throw new ArgumentException(e.Message);
        }

        postToEdit.Title = title;
        postToEdit.Body = body;
        postRepo.UpdateAsync(postToEdit);
        return Task.CompletedTask;
    }
}