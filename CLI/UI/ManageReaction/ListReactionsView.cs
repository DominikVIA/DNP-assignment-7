using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageReaction;

public class ListReactionsView (IReactionRepository reactRepo)
{
    public IQueryable<Reaction> GetAllReactions ()
    {
        return reactRepo.GetMany();
    }
    
    public Task<Reaction> GetPost (int id)
    {
        return reactRepo.GetSingleAsync(id);
    }
}