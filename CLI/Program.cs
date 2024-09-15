using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;
using RepositoryVerificationProxies;

Console.WriteLine("Starting...");
IUserRepository userRepo = new UserVerificationProxy(new UserInMemoryRepository());
IPostRepository postRepo = new PostVerificationProxy(new PostInMemoryRepository());
ICommentRepository commentRepo = new CommentInMemoryRepository();
IReactionRepository reactionRepo = new ReactionInMemoryRepository();

CliTerminal cliTerminal = new CliTerminal(userRepo, postRepo, commentRepo, reactionRepo);
await cliTerminal.StartAsync();