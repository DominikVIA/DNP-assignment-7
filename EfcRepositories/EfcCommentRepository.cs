using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }
    public async Task UpdateAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(p => p.Id == comment.Id)))
        {
            Console.WriteLine($"Comment with id {comment.Id} not found");
            throw new KeyNotFoundException($"Comment with the ID {comment.Id} not found");
        }

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            Console.WriteLine($"Comment with id {id} not found");
            throw new KeyNotFoundException($"Comment with the ID {id} not found");
        }

        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }
    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            Console.WriteLine($"Comment with id {id} not found");
            throw new KeyNotFoundException($"Comment with the ID {id} not found");
        }

        return existing;
    }
    public IQueryable<Comment> GetMany()
    {
        return ctx.Comments.AsQueryable();
    }


}