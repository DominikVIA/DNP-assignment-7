using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace RepositoryVerificationProxies;

public class UserVerificationProxy : IUserRepository
{
    private static UserInMemoryRepository? userRepo;

    public UserVerificationProxy(UserInMemoryRepository userRepo)
    {
        UserVerificationProxy.userRepo = userRepo;
    }

    public static Task<int> VerifyUserId(int id)
    {
        if(id < 0) throw new InvalidOperationException("ID cannot be less than zero.");
        // if (!id.All(char.IsDigit)) throw new ArgumentException("ID must be a number.");
        if(!userRepo.GetMany().Any(u => u.Id == id)) throw new InvalidOperationException("User with this ID does not exist.");
        return Task.FromResult(id);
    }
    
    private Task VerifyUsername(string? username)
    {
        if (username is null || username.Length == 0 || username.Equals("")) throw new InvalidOperationException("Username cannot be blank.");
        if (username.Contains(' ')) throw new InvalidOperationException("Username cannot whitespaces.");
        if(userRepo.GetMany().Any(u => u.Username.Equals(username))) throw new InvalidOperationException("Username already exists.");
        return Task.CompletedTask;
    }
    
    private Task VerifyPassword(string? password)
    {
        if (password is null || password.Length == 0 || password.Equals("")) throw new InvalidOperationException("Password cannot be blank.");
        if (password.Contains(' ')) throw new InvalidOperationException("Password cannot whitespaces.");
        return Task.CompletedTask;
    }

    public Task<User> AddAsync(User user)
    {
        VerifyUsername(user.Username);
        VerifyPassword(user.Password);
        return userRepo.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        VerifyUsername(user.Username);
        VerifyPassword(user.Password);
        return userRepo.UpdateAsync(user);
    }

    public Task DeleteAsync(int id)
    {
        return userRepo.DeleteAsync(VerifyUserId(id).Result);
    }

    public Task<User> GetSingleAsync(int id)
    {
        return userRepo.GetSingleAsync(VerifyUserId(id).Result);
    }

    public IQueryable<User> GetMany()
    {
        return userRepo.GetMany();
    }
}