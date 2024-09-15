using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace RepositoryVerificationProxies;

public class PostVerificationProxy : IPostRepository
{
    private static PostInMemoryRepository? postRepo;

    public PostVerificationProxy(PostInMemoryRepository postRepo)
    {
        PostVerificationProxy.postRepo = postRepo;
    }
    
    public static Task<int> VerifyPostId(int? id)
    {
        if (id is null) throw new InvalidOperationException("ID cannot be blank.");
        if(id < 0) throw new InvalidOperationException("ID cannot be less than zero.");
        // if (!id.All(char.IsDigit)) throw new ArgumentException("ID must be a number.");
        if(!postRepo.GetMany().Any(p => p.Id == id)) throw new InvalidOperationException("Post with this ID does not exist.");
        return Task.FromResult((int) id);
    }
    
    private Task VerifyTitle(string? title)
    {
        if (title is null || title.Length == 0 || title.Equals(" ")) throw new InvalidOperationException("Title cannot be blank.");
        return Task.CompletedTask;
    }
    
    private Task VerifyBody(string? body)
    {
        if (body is null || body.Length == 0 || body.Equals(" ")) throw new InvalidOperationException("Body cannot be blank.");
        return Task.CompletedTask;
    }

    public Task<Post> AddAsync(Post post)
    {
        UserVerificationProxy.VerifyUserId(post.AuthorId);
        VerifyTitle(post.Title);
        VerifyBody(post.Body);
        return postRepo.AddAsync(post);
    }

    public Task UpdateAsync(Post post)
    {
        VerifyTitle(post.Title);
        VerifyBody(post.Body);
        return postRepo.UpdateAsync(post);
    }

    public Task DeleteAsync(int id)
    {
        return postRepo.DeleteAsync(VerifyPostId(id).Result);
    }

    public Task<Post> GetSingleAsync(int id)
    {
        return postRepo.GetSingleAsync(VerifyPostId(id).Result);
    }

    public IQueryable<Post> GetMany()
    {
        return postRepo.GetMany();
    }
}