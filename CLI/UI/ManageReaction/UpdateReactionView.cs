namespace CLI.UI.ManageReaction;

using Entities;
using RepositoryContracts;

public class UpdateReactionView (IReactionRepository reactRepo)
{
    public Task DeleteReaction(int id)
    {
        reactRepo.DeleteAsync(id);
        return Task.CompletedTask;
    }
}