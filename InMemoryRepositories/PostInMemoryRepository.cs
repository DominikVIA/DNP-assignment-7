using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new();

    public PostInMemoryRepository()
    {
        DummyData();
    }

    private Task DummyData()
    {
        AddAsync(new Post(1, "Testing", "This is a test", DateTime.Now));
        AddAsync(new Post(1, "SEP logo", "Check out this amazing logo", DateTime.Now));
        AddAsync(new Post(2, "Going home", "I'm so tired", DateTime.Now));
        AddAsync(new Post(3, "At the gym", "Look at my amazing muscles", DateTime.Now));
        AddAsync(new Post(4, "Writing this", "The hardest part of programming", DateTime.Now));
        return Task.CompletedTask;
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' does not exist");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' does not exist");
        }
        posts.Remove(existingPost);
        
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' does not exist");
        }
        
        return Task.FromResult(existingPost);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}