using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageReaction;

public class ManageReactionView
{
    private CreateReactionView createReactionView;
    private ListReactionsView listReacionsView;
    private UpdateReactionView updateReacionView;
    
    
    public ManageReactionView(IReactionRepository reactRepo)
    {
        createReactionView = new CreateReactionView(reactRepo);
        listReacionsView = new ListReactionsView(reactRepo);
        updateReacionView = new UpdateReactionView(reactRepo);
    }
    
    public void Show()
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
            IQueryable<Reaction> reactions = listReacionsView.GetAllReactions();
            int id;
            switch (answer)
            {
                case 1:
                    listAllreactions();
                    break;
                
                case 2:
                    createReaction();
                    break; 
                
                case 3:
                    deleteReaction();
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

    public void listAllreactions()
    {
        IQueryable<Reaction> reactions = listReacionsView.GetAllReactions();
        Console.WriteLine("~~~~~~~~~~ Listing all reactions ~~~~~~~~~~");
        reactions = listReacionsView.GetAllReactions();
        foreach (var reaction in reactions)
        {
            Console.WriteLine("Reaction " + reaction.Id + ": " + "Content: " + 
                              reaction.ContentId + ", User: " + reaction.UserId);
        }
    }

    public void createReaction()
    {
        IQueryable<Reaction> reactions = listReacionsView.GetAllReactions();
        int userId;
        int contentId;
        bool like;
        bool dislike;
        Console.WriteLine("~~~~~~~~~~ Create new reaction ~~~~~~~~~~");
                    
        Console.WriteLine("Enter the ID of user reacting: ");
        userId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the ID of the content you are reacting to: ");
        contentId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Press 1 for like or 0 for dislike");
        int likeOrDislike = Convert.ToInt32(Console.ReadLine());

        if (likeOrDislike == 1)
        {
            like = true;
            dislike = false;
        } 
        else if (likeOrDislike == 0)
        {
            like = false;
            dislike = true;
        }
        else
        {
            Console.WriteLine("Please enter 1 or 0 to react to the content");
            Show();
            return;
        }
                    
        Reaction newReation = null;
        try
        {
            newReation = createReactionView.CreateReaction(userId, contentId, like, dislike, DateTime.Now).Result;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful reaction creation ~~~~~~~~~~" +
                          $"\nReaction ID: {newReation.Id}");
    }

    public void deleteReaction()
    {
        IQueryable<Reaction> reactions = listReacionsView.GetAllReactions();
        Console.WriteLine("~~~~~~~~~~ Delete an existing reaction ~~~~~~~~~~");
        
        foreach (var reaction in reactions)
        {
            Console.WriteLine("Reaction " + reaction.Id + ": " + "Content: " + 
                              reaction.ContentId + ", User: " + reaction.UserId);
        }
        Console.Write("Enter the ID of the post you want to update: ");
        string? readLine = Console.ReadLine();
                    
        if (readLine is null || readLine.Equals("") || readLine.Contains(' ') || !readLine.All(char.IsDigit)) 
        { 
            Console.WriteLine("ID cannot be blank and it must be a number."); 
        }
                                            
        int id = int.Parse(readLine);
        Reaction reactionToDelete = reactions.FirstOrDefault(u => u.Id == id);
        try
        {
            updateReacionView.DeleteReaction(id);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
                    
        Console.WriteLine("~~~~~~~~~~ Successful post deletion ~~~~~~~~~~" +
                          $"\nId: '{reactionToDelete.Id}'");
    }
}