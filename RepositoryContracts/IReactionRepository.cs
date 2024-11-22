using Entities;

namespace RepositoryContracts;

public interface IReactionRepository 
{
    Task<Reaction> AddAsync(Reaction reaction);
    /*Task<Reaction> UpdateAsync(Reaction reaction);*/
    Task DeleteAsync(int userId, int contentId);
    Task<Reaction> GetSingleAsync(int userId, int contentId);
    IQueryable<Reaction> GetMany();
}