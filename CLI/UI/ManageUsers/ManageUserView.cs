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

    public void Show()
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
                Show();
                return;
            }
            int answer = readLine[0] - '0';
            string? username;
            string? password;
            IQueryable<User> users;
            int id;
            switch (answer)
            {
                case 1:
                    Console.WriteLine("~~~~~~~~~~ Listing all users ~~~~~~~~~~");
                    users = listUsersView.GetAllUsers();
                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Id + " - " + user.Username + " " + user.Password);
                    }
                    break;
                case 2:
                    Console.WriteLine("~~~~~~~~~~ Creating new user ~~~~~~~~~~");
                    Console.Write("Username: ");
                    username = Console.ReadLine();
                    Console.Write("Password: ");
                    password = Console.ReadLine();

                    User newUser;
                    try
                    {
                        newUser = createUserView.CreateUser(username, password).Result;
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }

                    Console.WriteLine("~~~~~~~~~~ Successful user creation ~~~~~~~~~~" +
                                      $"\nUser ID: {newUser.Id}" +
                                      $"\nUsername: '{username}'" +
                                      $"\nPassword: '{password}'" );
                    break; 
                case 3:
                    Console.WriteLine("~~~~~~~~~~ Updating an existing user ~~~~~~~~~~");
                    
                    users = listUsersView.GetAllUsers();
                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Id + " - " + user.Username + " " + user.Password);
                    }
                    Console.Write("Enter the ID of the user you want to update: ");
                    readLine = Console.ReadLine();
                    string oldUsername;
                    string oldPassword;
                        
                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit))
                    {
                        Console.WriteLine("ID cannot be blank and it must be a number.");
                        break;
                    }
                    id = int.Parse(readLine);
                    User tempUser = users.FirstOrDefault(u => u.Id == id);
                    if (tempUser is null)
                    {
                        Console.WriteLine($"User with id '{id}' does not exist.");
                        break;
                    }
                    Console.Write("Enter new username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter new password: ");
                    password = Console.ReadLine();
                    oldUsername = tempUser.Username;
                    oldPassword = tempUser.Password;
                    
                    try
                    {
                        updateUserView.UpdateUser(id, username, password);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    
                    Console.WriteLine("~~~~~~~~~~ Successful user edit ~~~~~~~~~~" +
                                      $"\nUsername: '{oldUsername}' -> '{username}'" +
                                      $"\nPassword: '{oldPassword}' -> '{password}'" );

                    break;
                case 4:
                    Console.WriteLine("~~~~~~~~~~ Deleting an existing user ~~~~~~~~~~");
                    users = listUsersView.GetAllUsers();
                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Id + " - " + user.Username + " " + user.Password);
                    }
                    Console.Write("Enter the ID of the user you want to update: ");
                    readLine = Console.ReadLine();
                    
                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit))
                    {
                        Console.WriteLine("ID cannot be blank and it must be a number.");
                        break;
                    }
                    
                    id = int.Parse(readLine);
                    User userToDelete = users.FirstOrDefault(u => u.Id == id);
                    try
                    {
                        updateUserView.DeleteUser(id);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    
                    Console.WriteLine("~~~~~~~~~~ Successful user deletion ~~~~~~~~~~" +
                                      $"\nUsername: '{userToDelete.Username}'" +
                                      $"\nPassword: '{userToDelete.Password}'" );
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
    
}