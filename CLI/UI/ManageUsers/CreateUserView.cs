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

}