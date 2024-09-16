using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliTerminal(
    IUserRepository userRepo,
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IReactionRepository reactionRepo
    )
{
    private readonly ManageUserView manageUserView = new ManageUserView(userRepo);
    private readonly ManagePostView managePostView = new ManagePostView(postRepo);
    private readonly ManageCommentsView manageCommentsView = new ManageCommentsView(commentRepo);
    private readonly IReactionRepository reactionRepo = reactionRepo;

    public async Task StartAsync()
    {
        bool finished = false;
        do
        {
            string? readLine = Console.ReadLine();
            if (readLine is null || readLine.Length != 1 ||
                !readLine.All(char.IsDigit))
            {
                Console.WriteLine("Please enter a number from one of the options seen above.");
                StartAsync();
                return;
            }
            int answer = int.Parse(readLine);
            switch (answer)
            {
                case 1:
                    manageUserView.Show();
                    break;
                case 2: 
                    managePostView.Show();
                    break;
                case 3: 
                    await manageCommentsView.Show();
                    break;
                default:
                    finished = true;
                    break;
            }
        }
        while(!finished);
    }
}