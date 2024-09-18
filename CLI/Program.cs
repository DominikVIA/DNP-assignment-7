using CLI.UI;
using FileRepositories;
using RepositoryContracts;
using RepositoryVerificationProxies;

Console.WriteLine("Starting...");
IUserRepository userRepo = new UserVerificationProxy(new UserFileRepository());
IPostRepository postRepo = new PostVerificationProxy(new PostFileRepository());
ICommentRepository commentRepo = new CommentVerificationProxy(new CommentFileRepository());
IReactionRepository reactionRepo = new ReactionVerificationProxies(new ReactionFileRepository());

CliTerminal cliTerminal = new CliTerminal(userRepo, postRepo, commentRepo, reactionRepo);
await cliTerminal.StartAsync();
