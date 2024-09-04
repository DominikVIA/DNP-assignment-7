using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository(List<Comment> comments) : ICommentRepository
{
    private List<Comment> comments = comments;

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