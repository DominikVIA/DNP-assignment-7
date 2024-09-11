using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUserView (IUserRepository userRepo)
{
    private CreateUserView createUserView;
    private ListUsersView listUsersView;
    private UpdateUserView updateUserView;

    public Task StartAsync()
    {
        createUserView = new CreateUserView(userRepo);
        listUsersView = new ListUsersView(userRepo);
        updateUserView = new UpdateUserView(userRepo);
        
        return Task.CompletedTask;
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
            switch (answer)
            {
                case 1:
                    Console.WriteLine("~~~~~~~~~~ Listing all users ~~~~~~~~~~");
                    IQueryable<User> users = listUsersView.GetAllUsers();
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
                    Console.Write("Old username: ");
                    username = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyUsername(username);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    Console.Write("Old password: ");
                    password = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyPassword(password);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    Console.Write("New username: ");
                    string? nUsername = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyUsername(nUsername);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    Console.Write("New password: ");
                    string? nPassword = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyPassword(nPassword);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    User userToEdit;
                    
                    try
                    {
                        userToEdit = userRepo.GetSingleAsync(new User(username, password)).Result;
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    User editedUser = new User(nUsername, nPassword);
                    editedUser.Id = userToEdit.Id;
                    userRepo.UpdateAsync(editedUser);
                    
                    Console.WriteLine("~~~~~~~~~~ Successful user edit ~~~~~~~~~~" +
                                      $"\nUser ID: {userToEdit.Id}" +
                                      $"\nUsername: '{username}' -> '{nUsername}'" +
                                      $"\nPassword: '{password}' -> '{nPassword}'" );

                    break;
                case 4:
                    Console.WriteLine("~~~~~~~~~~ Deleting an existing user ~~~~~~~~~~");
                    Console.Write("Username: ");
                    username = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyUsername(username);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 4;
                    }
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    try
                    {
                        updateUserView.VerifyPassword(password);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 4;
                    }

                    User userToDelete;
                    try
                    {
                        userToDelete = userRepo.GetSingleAsync(new User(username, password)).Result;
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 4;
                    }
                    
                    userRepo.DeleteAsync(userToDelete.Id);
                    
                    Console.WriteLine("~~~~~~~~~~ Successful user deletion ~~~~~~~~~~" +
                                      $"\nUser ID: {userToDelete.Id}" +
                                      $"\nUsername: '{username}'" +
                                      $"\nPassword: '{password}'" );

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