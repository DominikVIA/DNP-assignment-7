using ApiContracts.Posts;

namespace BlazorApp1.Services;

public interface IPostService
{
    public Task<PostDto> GetSinglePostAsync(int id);
    public Task<PostDto> AddPostAsync(CreatePostDto postDto);
    public Task<PostDto> UpdatePostAsync(int id, UpdatePostDto postDto);
    public Task DeletePostAsync(int id);
    public Task<IQueryable<PostDto>> GetAllPostsAsync();
}