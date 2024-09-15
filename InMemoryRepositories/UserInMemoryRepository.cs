using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public UserInMemoryRepository()
    {
        DummyData();
    }
    
    private Task DummyData()
    {
        AddAsync(new User("Maria", "Yepez"));
        AddAsync(new User("Joan", "Hageneier"));
        AddAsync(new User("Sebastian", "Villarroel"));
        AddAsync(new User("Dominik", "Kielbowski"));
        return Task.CompletedTask;
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' does not exist");
        }
        users.Remove(existingUser);
        users.Add(user);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' does not exist");
        }
        users.Remove(existingUser);
        
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' does not exist");
        }
        
        return Task.FromResult(existingUser);
    }
    
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}