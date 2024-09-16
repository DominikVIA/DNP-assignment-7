using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageReaction;

public class ManageReactionView
{
    private CreateReactionView createReactionView;
    private ListReactionsView listReactionsView;
    private UpdateReactionView updateReactionView;
    
    
    public ManageReactionView(IReactionRepository reactRepo)
    {
        createReactionView = new CreateReactionView(reactRepo);
        listReactionsView = new ListReactionsView(reactRepo);
        updateReactionView = new UpdateReactionView(reactRepo);
    }
    
    public async Task Show()
    {
        bool finished = false;
        Console.WriteLine("Welcome to managing reactions. ");
        do
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                              "\nChoose the action you want to take:  " +
                              "\n1. List reactions." +
                              "\n2. Create a new reaction." +
                              //"\n3. Update an existing post." +
                              "\n3. Delete an existing reaction." +
                              "\n4. Go back to main menu.");
            string? readLine = Console.ReadLine();
            if (readLine is null || readLine.Length != 1 ||
                !readLine.All(char.IsDigit))
            {
                Console.WriteLine("Please enter a number from one of the options seen above.");
                Show();
                return;
            }
            int answer = readLine[0] - '0';
            switch (answer)
            {
                case 1:
                    await ListAllReactions();
                    break;
                
                case 2:
                    await CreateReaction();
                    break; 
                
                case 3:
                    await DeleteReaction();
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

    private Task ListAllReactions()
    {
        Console.WriteLine("~~~~~~~~~~ Listing all reactions ~~~~~~~~~~");
        IQueryable<Reaction> reactions = listReactionsView.GetAllReactions();
        foreach (var reaction in reactions)
        {
            Console.WriteLine("Reaction " + reaction.Id + ": " + "Content: " + 
                              reaction.ContentId + ", User: " + reaction.UserId +
                              (reaction.Like ? ", Like" : ", Dislike"));
        }
        return Task.CompletedTask;
    }

    private Task CreateReaction()
    {
        bool like;
        Console.WriteLine("~~~~~~~~~~ Create new reaction ~~~~~~~~~~");
                    
        Console.WriteLine("Enter the ID of user reacting: ");
        var userId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the ID of the content you are reacting to: ");
        var contentId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Press 1 for like or 0 for dislike");
        var readLine = Console.ReadLine();

        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int likeOrDislike))
        {
            Console.WriteLine("ID cannot be blank and it must be a number.");
            return Task.CompletedTask;
        }
        likeOrDislike = int.Parse(readLine);

        if (likeOrDislike is 1 or 0)
        {
            like = likeOrDislike == 1;
        } 
        else
        {
            Console.WriteLine("Please enter 1 or 0 to react to the content");
            return Task.CompletedTask;
        }
                    
        Reaction newReaction;
        try
        {
            newReaction = createReactionView.CreateReaction(userId, contentId, like, DateTime.Now).Result;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            return Task.CompletedTask;
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful reaction creation ~~~~~~~~~~" +
                          $"\nReaction ID: {newReaction.Id}");
        return Task.CompletedTask;
    }

    private Task DeleteReaction()
    {
        IQueryable<Reaction> reactions = listReactionsView.GetAllReactions();
        Console.WriteLine("~~~~~~~~~~ Delete an existing reaction ~~~~~~~~~~");
        
        foreach (var reaction in reactions)
        {
            Console.WriteLine("Reaction " + reaction.Id + ": " + "Content: " + 
                              reaction.ContentId + ", User: " + reaction.UserId);
        }
        Console.Write("Enter the ID of the post you want to update: ");
        string? readLine = Console.ReadLine();
                    
        if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
        }
                                            
        id = int.Parse(readLine);
        Reaction reactionToDelete = reactions.FirstOrDefault(u => u.Id == id);
        try
        {
            updateReactionView.DeleteReaction(id);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful post deletion ~~~~~~~~~~" +
                          $"\nId: '{reactionToDelete.Id}'");
        return Task.CompletedTask;
    }
}