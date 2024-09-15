using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private CreatePostView createPostView;
    private ListPostsView listPostsView;
    private UpdatePostView updatePostView;

    public ManagePostView(IPostRepository postRepo)
    {
        createPostView = new CreatePostView(postRepo);
        listPostsView = new ListPostsView(postRepo);
        updatePostView = new UpdatePostView(postRepo);
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
            int authorId;
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
                    readLine = Console.ReadLine();

                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit)) 
                    { 
                        Console.WriteLine("ID cannot be blank and it must be a number."); 
                        break;
                    }

                    id = int.Parse(readLine);
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
                    
                    Console.WriteLine("Enter the ID of the user creating the post: ");
                    readLine = Console.ReadLine();
                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit)) 
                    { 
                        Console.WriteLine("ID cannot be blank and it must be a number."); 
                        break;
                    }
                    authorId = int.Parse(readLine);
                    
                    Console.Write("Title: ");
                    title = Console.ReadLine();
                    
                    Console.Write("Post's text: ");
                    body = Console.ReadLine();

                    Post newPost;
                    try
                    {
                        newPost = createPostView.CreatePost(authorId, title, body).Result;
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                    
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
                    readLine = Console.ReadLine();
                    string oldTitle;
                    string oldBody;
                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit)) 
                    { 
                        Console.WriteLine("ID cannot be blank and it must be a number."); 
                        break;
                    }
                        
                    id = int.Parse(readLine);
                    Post tempPost = posts.FirstOrDefault(u => u.Id == id);
                    if (tempPost is null)
                    {
                        Console.WriteLine($"Post with id '{id}' does not exist.");
                        break;
                    }
                    Console.Write("Enter new title: ");
                    title = Console.ReadLine();
                    Console.Write("Enter new body: ");
                    body = Console.ReadLine();

                    oldTitle = tempPost.Title;
                    oldBody = tempPost.Body;
                    try
                    {
                        updatePostView.UpdatePost(id, title, body);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
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
                    readLine = Console.ReadLine();
                    
                    if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit)) 
                    { 
                        Console.WriteLine("ID cannot be blank and it must be a number."); 
                        break;
                    }
                                            
                    id = int.Parse(readLine);
                    Post postToDelete = posts.FirstOrDefault(u => u.Id == id);
                    try
                    {
                        updatePostView.DeletePost(id);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
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