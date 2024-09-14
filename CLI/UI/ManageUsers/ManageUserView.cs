using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUserView
{
    private CreateUserView createUserView;
    private ListUsersView listUsersView;
    private UpdateUserView updateUserView;
    private UserInfoVerification infoVerification;

    public ManageUserView(IUserRepository userRepo)
    {
        createUserView = new CreateUserView(userRepo);
        listUsersView = new ListUsersView(userRepo);
        updateUserView = new UpdateUserView(userRepo);
        infoVerification = new UserInfoVerification(userRepo);
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
            string? readId;
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

                    try
                    {
                        Console.Write("Username: ");
                        username = Console.ReadLine();
                        infoVerification.VerifyUsername(username);
                        Console.Write("Password: ");
                        password = Console.ReadLine();
                        infoVerification.VerifyPassword(password);
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
                    readId = Console.ReadLine();
                    string oldUsername;
                    string oldPassword;
                        
                    try
                    {
                        id = infoVerification.VerifyId(readId).Result;
                        Console.Write("Enter new username: ");
                        username = Console.ReadLine();
                        infoVerification.VerifyUsername(username);
                        Console.Write("Enter new password: ");
                        password = Console.ReadLine();
                        infoVerification.VerifyPassword(password);
                        User tempUser = users.FirstOrDefault(u => u.Id == id);
                        oldUsername = tempUser.Username;
                        oldPassword = tempUser.Password;
                        updateUserView.UpdateUser(id, username, password);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
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
                    readId = Console.ReadLine();

                    User userToDelete;
                    try
                    {
                        id = infoVerification.VerifyId(readId).Result;
                        userToDelete = users.FirstOrDefault(u => u.Id == id);
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