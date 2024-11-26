using System.Text.Json;
using ApiContracts.Reactions;

namespace BlazorApp1.Services;

public class HttpReactionService : IReactionService
{
    private readonly HttpClient _httpClient;

    public HttpReactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReactionDto> GetSingleReactionAsync(int userId, int contentId)
    {
        HttpResponseMessage httpResponse =
            await _httpClient.GetAsync(
                $"https://localhost:7065/Reactions/{userId}/{contentId}?includeUser=true&includeContent=true");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<ReactionDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<ReactionDto> AddReactionAsync(CreateReactionDto reactionDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("https://localhost:7065/Reactions", reactionDto); 
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        return JsonSerializer.Deserialize<ReactionDto>(
            response, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    } 

    public Task<ReactionDto> UpdateReactionAsync(int userId, int contentId, UpdateReactionDto reactionDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteReactionAsync(int userId, int contentId)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7065/Reactions/{userId}/{contentId}");
        response.EnsureSuccessStatusCode();
    }


    public async Task<IQueryable<ReactionDto>> GetAllReactionsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://localhost:7065/Reactions");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        List<ReactionDto> reactions = JsonSerializer.Deserialize<List<ReactionDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        
        return reactions.AsQueryable();
    }
}