using ApiContracts.Users;

namespace BlazorApp1.Services;

public interface IUserService
{
    public Task<UserDto> GetSingleUserAsync(int id);
    public Task<UserDto> AddUserAsync(CreateUserDto userDto);
    public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto);
    public Task DeleteUserAsync(int id);
    public Task<IQueryable<UserDto>> GetAllUsersAsync();
}