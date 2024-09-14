using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UserInfoVerification(IUserRepository userRepo)
{
    public Task<int> VerifyId(string? id)
    {
        if (id is null || id.Length == 0) throw new ArgumentException("ID cannot be blank.");
        if (!id.All(char.IsDigit)) throw new ArgumentException("ID must be a number.");
        int idParsed = int.Parse(id);
        if(!userRepo.GetMany().Any(u => u.Id == idParsed)) throw new ArgumentException("User with this ID does not exist.");
        return Task.FromResult(idParsed);
    }
    
    public Task VerifyUsername(string? username)
    {
        if (username is null || username.Length == 0) throw new ArgumentException("Username cannot be blank.");
        if(userRepo.GetMany().Any(u => u.Username.Equals(username))) throw new ArgumentException("Username already exists.");
        return Task.CompletedTask;
    }
    
    public Task VerifyPassword(string? password)
    {
        if (password is null || password.Length == 0) throw new ArgumentException("Password cannot be blank.");
        return Task.CompletedTask;
    }
}