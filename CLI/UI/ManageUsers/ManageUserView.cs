using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUserView
{
    private CreateUserView createUserView;
    private ListUsersView listUsersView;
    private UpdateUserView updateUserView;

    public ManageUserView(IUserRepository userRepo)
    {
        createUserView = new CreateUserView(userRepo);
        listUsersView = new ListUsersView(userRepo);
        updateUserView = new UpdateUserView(userRepo);
    }

    public async Task Show()
    {
        bool finished = false;
        Console.WriteLine("Welcome to managing users. ");
        do
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                              "\nChoose the action you want to take:  " +
                              "\n1. List users." +
                              "\n2. Create a new user." +
                              "\n3. Update an existing user." +
                              "\n4. Delete an existing user." +
                              "\n5. Go back to main menu.");
            string? readLine = Console.ReadLine();
            if (readLine is null || readLine.Length != 1 ||
                !readLine.All(char.IsDigit))
            {
                Console.WriteLine("Please enter a number from one of the options seen above.");
                await Show();
                return;
            }
            int answer = readLine[0] - '0';
            switch (answer)
            {
                case 1:
                    await ListAllUsers();
                    break;
                case 2:
                    await CreateUser();
                    break; 
                case 3:
                    await UpdateUser();
                    break;
                case 4:
                    await DeleteUser();
                    break;
                default:
                {
                    Console.WriteLine("Going back to main menu");
                    finished = true;
                    break;
                }
            }
        }
        while(!finished);
    }

    private async Task ListAllUsers()
    {
        Console.WriteLine("~~~~~~~~~~ Listing all users ~~~~~~~~~~");
        var users = listUsersView.GetAllUsers();
        foreach (var user in users)
        {
            Console.WriteLine(user.Id + " - " + user.Username + " " + user.Password);
        }
    }

    private async Task CreateUser()
    {
        Console.WriteLine("~~~~~~~~~~ Creating new user ~~~~~~~~~~");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        User newUser;
        try
        {
            newUser = createUserView.CreateUser(username, password).Result;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        Console.WriteLine("~~~~~~~~~~ Successful user creation ~~~~~~~~~~" +
                          $"\nUser ID: {newUser.Id}" +
                          $"\nUsername: '{username}'" +
                          $"\nPassword: '{password}'" );
    }

    private async Task UpdateUser()
    {
        Console.WriteLine("~~~~~~~~~~ Updating an existing user ~~~~~~~~~~");

        await ListAllUsers();
        Console.Write("Enter the ID of the user you want to update: ");
        var readLine = Console.ReadLine();

        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id))
        {
            Console.WriteLine("ID cannot be blank and it must be a number.");
            return;
        }
        id = int.Parse(readLine);
        var tempUser = listUsersView.GetAllUsers().FirstOrDefault(u => u.Id == id);
        if (tempUser is null)
        {
            Console.WriteLine($"User with id '{id}' does not exist.");
            return;
        }
        Console.Write("Enter new username: ");
        var username = Console.ReadLine();
        Console.Write("Enter new password: ");
        var password = Console.ReadLine();
        var oldUsername = tempUser.Username;
        var oldPassword = tempUser.Password;
                    
        try
        {
            updateUserView.UpdateUser(id, username, password);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful user edit ~~~~~~~~~~" +
                          $"\nUsername: '{oldUsername}' -> '{username}'" +
                          $"\nPassword: '{oldPassword}' -> '{password}'" );
    }

    private async Task DeleteUser()
    {
        Console.WriteLine("~~~~~~~~~~ Deleting an existing user ~~~~~~~~~~");
        await ListAllUsers();
        Console.Write("Enter the ID of the user you want to update: ");
        var readLine = Console.ReadLine();
                    
        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id))
        {
            Console.WriteLine("ID cannot be blank and it must be a number.");
            return;
        }
                    
        id = int.Parse(readLine);
        var tempUser = listUsersView.GetAllUsers().FirstOrDefault(u => u.Id == id);
        try
        {
            updateUserView.DeleteUser(id);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful user deletion ~~~~~~~~~~" +
                          $"\nUsername: '{tempUser.Username}'" +
                          $"\nPassword: '{tempUser.Password}'" );
    }
}