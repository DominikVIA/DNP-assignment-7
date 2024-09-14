using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView (IPostRepository postRepo)
{
    public IQueryable<Post> GetAllPosts ()
    {
        return postRepo.GetMany();
    }
    
    public Task<Post> GetPost (int id)
    {
        return postRepo.GetSingleAsync(id);
    }
}