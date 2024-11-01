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

    public Task DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<CommentDto>> GetAllCommentsAsync()
    {
        throw new NotImplementedException();
    }
}