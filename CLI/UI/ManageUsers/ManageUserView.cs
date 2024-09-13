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
            if (readLine is null || readLine.Length != 1 || !readLine.All(char.IsDigit))
                throw new ArgumentException("The provided string is invalid");
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
                    try
                    {
                        if (!createUserView.VerifyUsername(username).Result)
                        {
                            Console.WriteLine("Username is already taken.");
                            goto case 2;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 2;
                    }
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    try
                    {
                        if (createUserView.VerifyPassword(password).Result)
                        {
                            
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 2;
                    }
                    User newUser = createUserView.CreateUser(username, password).Result;
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
                    id = Console.ReadLine()[0] - '0';
                    
                    User userToEdit = users.FirstOrDefault(u => u.Id == id);
                    
                    try
                    {
                        Console.Write("Enter new username: ");
                        username = Console.ReadLine();
                        updateUserView.VerifyUsername(username);
                        Console.Write("Enter new password: ");
                        password = Console.ReadLine();
                        updateUserView.VerifyPassword(password);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    try
                    {
                        updateUserView.UpdateUser(id, username, password);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    Console.WriteLine("~~~~~~~~~~ Successful user edit ~~~~~~~~~~" +
                                      $"\nUsername: '{userToEdit.Username}' -> '{username}'" +
                                      $"\nPassword: '{userToEdit.Password}' -> '{password}'" );

                    break;
                case 4:
                    Console.WriteLine("~~~~~~~~~~ Deleting an existing user ~~~~~~~~~~");
                    users = listUsersView.GetAllUsers();
                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Id + " - " + user.Username + " " + user.Password);
                    }
                    Console.Write("Enter the ID of the user you want to update: ");
                    id = Console.ReadLine()[0] - '0';
                    
                    User userToDelete = users.FirstOrDefault(u => u.Id == id);
                    
                    try
                    {
                        updateUserView.DeleteUser(id);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 4;
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