using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UpdateUserView (IUserRepository userRepo)
{
    public Task DeleteUser(int id)
    {
        userRepo.DeleteAsync(id);
        return Task.CompletedTask;
    }
    
    public Task UpdateUser(string username, string password)
    {
        userRepo.UpdateAsync(new User(username, password));
        return Task.CompletedTask;
    }
    
    public Task VerifyUsername(string? username)
    {
        if (username is null || username.Length == 0) throw new ArgumentException("Username cannot be blank.");
        return Task.CompletedTask;
    }
    
    public Task VerifyPassword(string? password)
    {
        if (password is null || password.Length == 0) throw new ArgumentException("Password cannot be blank.");
        return Task.CompletedTask;
    }
}