using System.Text.Json;
using ApiContracts.Users;

namespace BlazorApp1.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto> GetSingleUserAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://localhost:7065/Users/{id}");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        return JsonSerializer.Deserialize<UserDto>(
            response, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;    
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("https://localhost:7065/Users", request); 
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        return JsonSerializer.Deserialize<UserDto>(
            response, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    } 

    public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IQueryable<UserDto>> GetAllUsersAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"https://localhost:7065/Users");
        httpResponse.EnsureSuccessStatusCode();
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        } 
        List<UserDto> users = JsonSerializer.Deserialize<List<UserDto>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return users.AsQueryable();
    }
}