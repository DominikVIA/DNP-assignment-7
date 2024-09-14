using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView (IPostRepository postRepo)
{
    public Task<Post> CreatePost(int authorId, string title, string body)
    {
        Post post = postRepo.AddAsync(new Post(authorId, title, body, DateTime.Now)).Result;
        return Task.FromResult(post);
    }
}