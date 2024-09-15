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
        User userToEdit = new User(username,password);
        userToEdit.Id = id;
        userRepo.UpdateAsync(userToEdit);
        return Task.CompletedTask;
    }
}