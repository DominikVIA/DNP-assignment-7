using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly CreatePostView createPostView;
    private readonly ListPostsView listPostsView;
    private readonly UpdatePostView updatePostView;

    public ManagePostView(IPostRepository postRepo)
    {
        createPostView = new CreatePostView(postRepo);
        listPostsView = new ListPostsView(postRepo);
        updatePostView = new UpdatePostView(postRepo);
    }

    public async Task Show()
    {
        bool finished = false;
        Console.WriteLine("Welcome to managing posts. ");
        do
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                              "\nChoose the action you want to take:  " +
                              "\n1 List posts." +
                              "\n2 See all information about a post." +
                              "\n3 Create a new post." +
                              "\n4 Update an existing post." +
                              "\n5 Delete an existing post." +
                              "\n< Go back to main menu.");
            string? readLine = Console.ReadLine();
            switch (readLine)
            {
                case "1":
                    await ListAllPosts();
                    break;
                case "2":
                    await ListPostInformation();
                    break;
                case "3":
                    await CreatePost();
                    break; 
                case "4":
                    await UpdatePost();
                    break;
                case "5":
                    await DeletePost();
                    break;
                case "<":
                    Console.WriteLine("Going back to main menu");
                    finished = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please choose one of the options above.");
                    break;
            }
        }
        while(!finished);
    }

    private Task ListAllPosts()
    {
        Console.WriteLine("~~~~~~~~~~ Listing all posts ~~~~~~~~~~");
        IQueryable<Post> posts = listPostsView.GetAllPosts();
        foreach (var post in posts)
        {
            Console.WriteLine(post.Title + " (" + post.Id + ")");
        }
        return Task.CompletedTask;
    }

    private async Task ListPostInformation()
    {
        Console.WriteLine("~~~~~~~~~~ Listing post information ~~~~~~~~~~");
        await ListAllPosts();
        string? readLine = Console.ReadLine();

        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
            return;
        }

        id = int.Parse(readLine);
        Post gottenPost;
        try
        {
            gottenPost = await listPostsView.GetPost(id);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        
        
        Console.WriteLine($"""
                             ID: {gottenPost.Id}
                             Written by: {gottenPost.AuthorId}
                             Title: {gottenPost.Title}
                             Body: 
                             {gottenPost.Body}
                             Created on: {gottenPost.DateCreated}
                             """
        );
    }

    private Task CreatePost()
    {
        Console.WriteLine("~~~~~~~~~~ Creating new post ~~~~~~~~~~");
                    
        Console.WriteLine("Enter the ID of the user creating the post: ");
        string? readLine = Console.ReadLine();
        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
            return Task.CompletedTask;
        }
        id = int.Parse(readLine);
                    
        Console.Write("Title: ");
        var title = Console.ReadLine();
                    
        Console.Write("Post's text: ");
        var body = Console.ReadLine();

        Post newPost;
        try
        {
            newPost = createPostView.CreatePost(id, title, body).Result;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return Task.CompletedTask;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful post creation ~~~~~~~~~~" +
                          $"\nPost ID: {newPost.Id}" +
                          $"\nTitle: '{title}'" +
                          $"\nBody: '{body}'" );
        return Task.CompletedTask;
    }

    private async Task UpdatePost()
    {
        Console.WriteLine("~~~~~~~~~~ Updating an existing post ~~~~~~~~~~");
        await ListAllPosts();
                    
        Console.Write("Enter the ID of the post you want to update: ");
        string? readLine = Console.ReadLine();
        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
            return;
        }
                        
        id = int.Parse(readLine);
        Post? tempPost = listPostsView.GetAllPosts().FirstOrDefault(u => u.Id == id);
        if (tempPost is null) 
        {
            Console.WriteLine($"Post with id '{id}' does not exist.");
            return;
        }
        Console.Write("Enter new title: ");
        string? title = Console.ReadLine();
        Console.Write("Enter new body: ");
        string? body = Console.ReadLine();

        var oldTitle = tempPost.Title;
        var oldBody = tempPost.Body;
        try
        {
            await updatePostView.UpdatePost(id, title, body);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful post edit ~~~~~~~~~~" +
                          $"\nTitle: '{oldTitle}' -> '{title}'" +
                          $"\nBody: '{oldBody}' -> '{body}'" );
    }

    private async Task DeletePost() {
        Console.WriteLine("~~~~~~~~~~ Deleting an existing post ~~~~~~~~~~");
        await ListAllPosts();
        Console.Write("Enter the ID of the post you want to update: ");
        var readLine = Console.ReadLine();
        
        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
            return;
        }
                                            
        id = int.Parse(readLine);
        var postToDelete = listPostsView.GetAllPosts().FirstOrDefault(u => u.Id == id);
        try
        {
            await updatePostView.DeletePost(id);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful post deletion ~~~~~~~~~~" +
                          $"\nTitle: '{postToDelete.Title}'" +
                          $"\nPassword: '{postToDelete.Body}'" );

    }
}