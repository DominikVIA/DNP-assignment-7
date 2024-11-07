namespace BlazorApp1.Auth;

using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiContracts.Users;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentClaimsPrincipal = new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentClaimsPrincipal));
    }

    public async Task Login(string userName, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", new LoginRequest { UserName = userName, Password = password });
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Invalid login attempt");
        }

        var userDto = await response.Content.ReadFromJsonAsync<SimpleUserDto>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("Id", userDto.Id.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, "apiauth");
        _currentClaimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));
    }

    public void Logout()
    {
        _currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));
    }
}
