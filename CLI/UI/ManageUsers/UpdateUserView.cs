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
    
    public Task UpdateUser(int id, string username, string password)
    {
        User userToEdit;
        try
        {
            userToEdit = userRepo.GetSingleAsync(id).Result;
        }
        catch(InvalidOperationException e)       {
            throw new InvalidOperationException(e.Message);
        }

        userToEdit.Username = username;
        userToEdit.Password = password;
        userRepo.UpdateAsync(userToEdit);
        return Task.CompletedTask;
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