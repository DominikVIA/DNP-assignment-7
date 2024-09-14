using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private CreatePostView createPostView;
    private ListPostsView listPostsView;
    private UpdatePostView updatePostView;
    private PostInfoVerification postInfoVerification;

    public ManagePostView(IPostRepository postRepo)
    {
        createPostView = new CreatePostView(postRepo);
        listPostsView = new ListPostsView(postRepo);
        updatePostView = new UpdatePostView(postRepo);
        postInfoVerification = new PostInfoVerification(postRepo);
    }

    public void Show()
    {
        bool finished = false;
        Console.WriteLine("Welcome to managing posts. ");
        do
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                              "\nChoose the action you want to take:  " +
                              "\n1. List posts." +
                              "\n2. See all information about a post." +
                              "\n3. Create a new post." +
                              "\n4. Update an existing post." +
                              "\n5. Delete an existing post." +
                              "\n6. Go back to main menu.");
            string? readLine = Console.ReadLine();
            if (readLine is null || readLine.Length != 1 ||
                !readLine.All(char.IsDigit))
            {
                Console.WriteLine("Please enter a number from one of the options seen above.");
                Show();
                return;
            }
            int answer = readLine[0] - '0';
            string? title;
            string? body;
            IQueryable<Post> posts = listPostsView.GetAllPosts();
            int id;
            int authorId = 1;
            string? readId;
            string? readAuthorId;
            switch (answer)
            {
                case 1:
                    Console.WriteLine("~~~~~~~~~~ Listing all posts ~~~~~~~~~~");
                    posts = listPostsView.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine(post.Title + " (" + post.Id + ")");
                    }
                    break;
                case 2:
                    Console.WriteLine("~~~~~~~~~~ Listing post information ~~~~~~~~~~");
                    posts = listPostsView.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine(post.Title + " (" + post.Id + ")");
                    }
                    Console.WriteLine("Enter the id of the post you want to fully display: ");
                    readId = Console.ReadLine();
                    try
                    {
                        id = postInfoVerification.VerifyId(readId).Result;
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e);
                        goto case 2;
                    }

                    Post gottenPost = listPostsView.GetPost(id).Result;
                    
                    Console.WriteLine($"ID: {gottenPost.Id}"+
                                      $"\nWritten by: {gottenPost.AuthorId}" +
                                      $"\nTitle: {gottenPost.Title}" +
                                      $"\nBody: \n{gottenPost.Body}" +
                                      $"\nLikes: {gottenPost.Likes.Count}" +
                                      $"\nDislikes: {gottenPost.Dislikes.Count}" +
                                      $"\nCreated on: {gottenPost.DateCreated}"
                                      );
                    
                    break;
                case 3:
                    Console.WriteLine("~~~~~~~~~~ Creating new post ~~~~~~~~~~");

                    try
                    {
                        Console.WriteLine("Enter the ID of the user creating the post: ");
                        readAuthorId = Console.ReadLine();
                        
                        Console.Write("Title: ");
                        title = Console.ReadLine();
                        postInfoVerification.VerifyTitle(title);
                        Console.Write("Post's text: ");
                        body = Console.ReadLine();
                        postInfoVerification.VerifyBody(body);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 3;
                    }
                    
                    Post newPost = createPostView.CreatePost(authorId, title, body).Result;
                    Console.WriteLine("~~~~~~~~~~ Successful post creation ~~~~~~~~~~" +
                                      $"\nPost ID: {newPost.Id}" +
                                      $"\nPostname: '{title}'" +
                                      $"\nPassword: '{body}'" );
                    break; 
                case 4:
                    Console.WriteLine("~~~~~~~~~~ Updating an existing post ~~~~~~~~~~");
                    
                    posts = listPostsView.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine(post.Title + " (" + post.Id + ")");
                    }
                    Console.Write("Enter the ID of the post you want to update: ");
                    readId = Console.ReadLine();
                    string oldTitle;
                    string oldBody;
                        
                    try
                    {
                        id = postInfoVerification.VerifyId(readId).Result;
                        Console.Write("Enter new title: ");
                        title = Console.ReadLine();
                        postInfoVerification.VerifyTitle(title);
                        Console.Write("Enter new body: ");
                        body = Console.ReadLine();
                        postInfoVerification.VerifyBody(body);
                        Post tempPost = posts.FirstOrDefault(u => u.Id == id);
                        oldTitle = tempPost.Title;
                        oldBody = tempPost.Body;
                        updatePostView.UpdatePost(id, title, body);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 4;
                    }
                    
                    Console.WriteLine("~~~~~~~~~~ Successful post edit ~~~~~~~~~~" +
                                      $"\nTitle: '{oldTitle}' -> '{title}'" +
                                      $"\nBody: '{oldBody}' -> '{body}'" );

                    break;
                case 5:
                    Console.WriteLine("~~~~~~~~~~ Deleting an existing post ~~~~~~~~~~");
                    posts = listPostsView.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine(post.Title + " (" + post.Id + ")");
                    }
                    Console.Write("Enter the ID of the post you want to update: ");
                    readId = Console.ReadLine();

                    Post postToDelete;
                    try
                    {
                        id = postInfoVerification.VerifyId(readId).Result;
                        postToDelete = posts.FirstOrDefault(u => u.Id == id);
                        updatePostView.DeletePost(id);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        goto case 5;
                    }
                    
                    Console.WriteLine("~~~~~~~~~~ Successful post deletion ~~~~~~~~~~" +
                                      $"\nTitle: '{postToDelete.Title}'" +
                                      $"\nPassword: '{postToDelete.Body}'" );

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