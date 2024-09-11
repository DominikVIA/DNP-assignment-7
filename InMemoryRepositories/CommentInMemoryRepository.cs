using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        DummyData();
    }
    
    private Task DummyData()
    {
        AddAsync(new Comment(2, 2, "I hate it because you made it", DateTime.Now));
        AddAsync(new Comment(4, 2, "I dont like that empty space", DateTime.Now));
        AddAsync(new Comment(3, 2, "me too", DateTime.Now));
        return Task.CompletedTask;
    }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' does not exist");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' does not exist");
        }
        comments.Remove(existingComment);
        
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' does not exist");
        }
        
        return Task.FromResult(existingComment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}