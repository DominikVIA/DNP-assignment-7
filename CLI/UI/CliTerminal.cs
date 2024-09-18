using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageReaction;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliTerminal(
    IUserRepository userRepo,
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IReactionRepository reactionRepo
    )
{
    private readonly ManageUserView manageUserView = new(userRepo);
    private readonly ManagePostView managePostView = new(postRepo);
    private readonly ManageCommentsView manageCommentsView = new(commentRepo);
    private readonly ManageReactionView manageReactionView = new(reactionRepo);

    public async Task StartAsync()
    {
        bool finished = false;
        do
        {
            // Console.Clear();
            Console.WriteLine("Welcome to the main menu, please choose one of the following options: " +
                              "\n 1 Manage users, \n 2 Manage posts, \n 3 Manage comments, \n 4 Manage reactions, \n < Exit program");
            string? readLine = Console.ReadLine();
            switch (readLine)
            {
                case "1":
                    await manageUserView.Show();
                    break;
                case "2": 
                    await managePostView.Show();
                    break;
                case "3": 
                    await manageCommentsView.Show();
                    break;
                case "4": 
                    await manageReactionView.Show();
                    break;
                case "<":
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please choose one of the options above.");
                    break;
            }
        }
        while(!finished);
    }
}