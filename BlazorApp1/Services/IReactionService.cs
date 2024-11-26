using ApiContracts.Reactions;

namespace BlazorApp1.Services;

public interface IReactionService
{
    public Task<ReactionDto> GetSingleReactionAsync(int userId, int contentId);
    public Task<ReactionDto> AddReactionAsync(CreateReactionDto reactionDto);
    public Task<ReactionDto> UpdateReactionAsync(int userId, int contentId, UpdateReactionDto reactionDto);
    public Task DeleteReactionAsync(int userId, int contentId);
    public Task<IQueryable<ReactionDto>> GetAllReactionsAsync();
}