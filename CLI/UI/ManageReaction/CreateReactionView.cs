using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageReaction;

public class CreateReactionView (IReactionRepository reactRepo)
{
    public Task<Reaction> CreateReaction(int userId, int contentId, bool like, DateTime dateCreated)
    {
        Reaction reaction = reactRepo.AddAsync(new Reaction(userId, contentId, like, dateCreated)).Result;
        return Task.FromResult(reaction);
    }
}