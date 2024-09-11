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
    private readonly IPostRepository postRepo = postRepo;
    private readonly ICommentRepository commentRepo = commentRepo;
    private readonly IReactionRepository reactionRepo = reactionRepo;

    public async Task<Task> StartAsync()
    {
        ManageUserView manageUserView = new ManageUserView(userRepo);
        await manageUserView.StartAsync();
        manageUserView.Show();

        return Task.CompletedTask;
    }
}