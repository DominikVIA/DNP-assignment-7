namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; }
    public string Password { get; }
    
    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Id = -1;
    }
}