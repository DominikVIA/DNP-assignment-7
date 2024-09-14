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
}