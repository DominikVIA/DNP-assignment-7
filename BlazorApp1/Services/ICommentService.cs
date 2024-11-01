using ApiContracts.Comments;

namespace BlazorApp1.Services;

public interface ICommentService
{
    public Task<CommentDto> GetSingleCommentAsync(int id);
    public Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto);
    public Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto commentDto);
    public Task DeleteCommentAsync(int id);
    public Task<IQueryable<CommentDto>> GetAllCommentsAsync();
}