using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView (IUserRepository userRepo)
{
    public IQueryable<User> GetAllUsers ()
    {
        return userRepo.GetMany();
    }
}