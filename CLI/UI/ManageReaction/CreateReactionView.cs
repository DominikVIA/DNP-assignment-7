using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageReaction;

public class CreateReactionView (IReactionRepository reactRepo)
{
    public Task<Reaction> CreateReaction(int userId, int contentId, bool like, bool dislike, DateTime dateCreated)
    {
        Reaction reaction = reactRepo.AddAsync(new Reaction(userId, contentId, like, dislike, dateCreated)).Result;
        return Task.FromResult(reaction);
    }
}