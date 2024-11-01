using System.Text.Json;
using ApiContracts.Posts;
using ApiContracts.Users;

namespace BlazorApp1.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PostDto> GetSinglePostAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await _httpClient.GetAsync(
                $"https://localhost:7065/Posts/{id}?includeComments=true&includeAuthor=true");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto postDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("https://localhost:7065/Posts", postDto); 
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        return JsonSerializer.Deserialize<PostDto>(
            response, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    } 

    public Task<PostDto> UpdatePostAsync(int id, UpdatePostDto postDto)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IQueryable<PostDto>> GetAllPostsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://localhost:7065/Posts");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        List<PostDto> posts = JsonSerializer.Deserialize<List<PostDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        
        return posts.AsQueryable();
    }
}