using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcReactionRepository : IReactionRepository
{
    private readonly AppContext ctx;

    public EfcReactionRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }
    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        EntityEntry<Reaction> entityEntry = await ctx.Reactions.AddAsync(reaction);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }
    /*public async Task UpdateAsync(Reaction reaction)
    {
        if (!(await ctx.Reactions.AnyAsync(p => new { p.UserId, p.ContentId } == id)))
        {
            Console.WriteLine($"Reaction with id {reaction}.Id}} not found");
        }

        ctx.Reactions.Update(reaction);
        await ctx.SaveChangesAsync();
    }*/
    public async Task DeleteAsync(int userId, int contentId)
    {
        Reaction? existing = await ctx.Reactions.SingleOrDefaultAsync(r => userId == r.UserId && contentId == r.ContentId);
        if (existing == null)
        {
            Console.WriteLine($"Reaction made by the {userId} for the content {contentId} not found");
        }

        ctx.Reactions.Remove(existing);
        await ctx.SaveChangesAsync();
    }
    public async Task<Reaction> GetSingleAsync(int userId, int contentId)
    {
        Reaction? existing = await ctx.Reactions.SingleOrDefaultAsync(r => userId == r.UserId && contentId == r.ContentId);
        if (existing == null)
        {
            Console.WriteLine($"Reaction made by the {userId} for the content {contentId} not found");
            throw new KeyNotFoundException($"Reaction made by the {userId} for the content {contentId} not found");
        }
        return existing;
    }
    public IQueryable<Reaction> GetMany()
    {
        return ctx.Reactions.AsQueryable();
    }


}