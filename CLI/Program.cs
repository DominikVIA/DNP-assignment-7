using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting...");
IUserRepository userRepo = new UserInMemoryRepository();
IPostRepository postRepo = new PostInMemoryRepository();
ICommentRepository commentRepo = new CommentInMemoryRepository();
IReactionRepository reactionRepo = new ReactionInMemoryRepository();

CliTerminal cliTerminal = new CliTerminal(userRepo, postRepo, commentRepo, reactionRepo);
await cliTerminal.StartAsync();