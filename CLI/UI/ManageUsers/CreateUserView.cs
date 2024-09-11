using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView (IUserRepository userRepo)
{
    public Task<User> CreateUser(string username, string password)
    {
        User user = userRepo.AddAsync(new User(username, password)).Result;
        return Task.FromResult(user);
    }

    public Task<bool> VerifyUsername(string? username)
    {
        if (username is null || username.Length == 0) throw new ArgumentException("Username cannot be blank.");
        IQueryable<User> users = userRepo.GetMany().Where(u => u.Username.Equals(username));
        return Task.FromResult(!users.Any());
    }
    
    public Task<bool> VerifyPassword(string? password)
    {
        if (password is null || password.Length == 0) throw new ArgumentException("Password cannot be blank.");
        // IQueryable<User> users = userRepo.GetMany().Where(u => u.Username.Equals(username));
        return Task.FromResult(true);
    }
}