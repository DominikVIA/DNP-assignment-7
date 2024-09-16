using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace RepositoryVerificationProxies;

public class ReactionVerificationProxies : IReactionRepository
{
    private static ReactionInMemoryRepository reactionRepo;

    public ReactionVerificationProxies(ReactionInMemoryRepository reactionRepo)
    {
        ReactionVerificationProxies.reactionRepo = reactionRepo;
    }
    
    public static Task<int> VerifyReactionId(int? id)
    {
        if (id is null) throw new ArgumentException("ID cannot be blank.");
        if (id < 0) throw new ArgumentException("ID cannot be less than zero.");
        // if (!id.All(char.IsDigit)) throw new ArgumentException("ID must be a number.");
        if (!reactionRepo.GetMany().Any(r => r.Id == id))
            throw new ArgumentException("Reaction with this ID does not exist.");
        return Task.FromResult((int)id);
    }

    private Task VerifyContentId(int? id)
    {
        if (id is null) throw new ArgumentException("ID cannot be blank.");
        try
        {
            PostVerificationProxy.VerifyPostId(id);
        }
        catch (ArgumentException eP)
        {
            try
            {
                CommentVerificationProxy.VerifyCommentId(id);
            }
            catch (ArgumentException eC)
            {
                throw new ArgumentException(eC.Message);
            }
        }
        return Task.CompletedTask;
    }

    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        await UserVerificationProxy.VerifyUserId(reaction.UserId);
        await VerifyContentId(reaction.ContentId);
        return await reactionRepo.AddAsync(reaction);
    }

    public async Task UpdateAsync(Reaction reaction)
    {
        await reactionRepo.UpdateAsync(reaction);
    }

    public async Task DeleteAsync(int id)
    {
        await reactionRepo.DeleteAsync(await VerifyReactionId(id));
    }

    public async Task<Reaction> GetSingleAsync(int id)
    {
        return await reactionRepo.GetSingleAsync(await VerifyReactionId(id));
    }

    public IQueryable<Reaction> GetMany()
    {
        return reactionRepo.GetMany();
    }
}