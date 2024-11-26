using System.Text.Json;
using ApiContracts.Comments;
using ApiContracts.Posts;

namespace BlazorApp1.Services;

public class HttpCommentService : ICommentService 
{
    private readonly HttpClient _httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<CommentDto> GetSingleCommentAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await _httpClient.GetAsync(
                $"https://localhost:7065/Comments/{id}?includeAuthor=true&includeParentContent=true");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("https://localhost:7065/Comments", commentDto); 
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        return JsonSerializer.Deserialize<CommentDto>(
            response, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

    }

    public Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto commentDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7065/Comments/{id}");
        response.EnsureSuccessStatusCode();
        
    }


    public async Task<IQueryable<CommentDto>> GetAllCommentsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("https://localhost:7065/Comments");
        httpResponse.EnsureSuccessStatusCode();

        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        List<CommentDto> comments = JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        return comments.AsQueryable();
    }

    public async Task<IQueryable<CommentDto>> GetAllCommentsByPostIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://localhost:7065/Comments/ByPostId?postId={postId}");
        httpResponse.EnsureSuccessStatusCode();

        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        List<CommentDto> comments = JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        return comments.AsQueryable();
    }
}