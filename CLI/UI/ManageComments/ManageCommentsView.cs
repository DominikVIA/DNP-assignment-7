using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments
{
    public class ManageCommentsView
    {
        private readonly CreateCommentView createCommentView;
        private readonly ListCommentsView listCommentsView;
        private readonly UpdateCommentView updateCommentView;

        public ManageCommentsView(ICommentRepository commentRepo)
        {
            createCommentView = new CreateCommentView(commentRepo);
            listCommentsView = new ListCommentsView(commentRepo);
            updateCommentView = new UpdateCommentView(commentRepo);
        }

        public async Task Show()
        {
            bool finished = false;
            Console.WriteLine("Welcome to managing comments.");
            do
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                                  "\nChoose the action you want to take: " +
                                  "\n1 List comments." +
                                  "\n2 See all information about a comment." +
                                  "\n3 Create a new comment." +
                                  "\n4 Update an existing comment." +
                                  "\n5 Delete an existing comment." +
                                  "\n< Go back to main menu.");
                string? readLine = Console.ReadLine();
                switch (readLine)
                {
                    case "1":
                        await ListCommentsAsync();
                        break;
                    case "2":
                        await ViewCommentDetailsAsync();
                        break;
                    case "3":
                        await CreateCommentAsync();
                        break;
                    case "4":
                        await UpdateCommentAsync();
                        break;
                    case "5":
                        await DeleteCommentAsync();
                        break;
                    case "<":
                        Console.WriteLine("Going back to main menu.");
                        finished = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose one of the options above.");
                        break;
                }
            } while (!finished);
        }

        private async Task ListCommentsAsync()
        {
            Console.WriteLine("~~~~~~~~~~ Listing all comments ~~~~~~~~~~");
            var comments = listCommentsView.GetAllComments();
            foreach (var comment in comments)
            {
                Console.WriteLine($"Comment ID: {comment.Id}, Author ID: {comment.AuthorId}, Responding To Post ID: {comment.RespondingToId}, Body: {comment.Body}");
            }
        }

        private async Task ViewCommentDetailsAsync()
        {
            Console.WriteLine("~~~~~~~~~~ Viewing comment information ~~~~~~~~~~");
            await ListCommentsAsync();
            Console.WriteLine("Enter the ID of the comment you want to fully display: ");
            string? readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id))
            {
                Console.WriteLine("ID cannot be blank and it must be a number.");
                return;
            }

            try
            {
                var comment = await listCommentsView.GetComment(id);
                Console.WriteLine($"ID: {comment.Id}" +
                                  $"\nWritten by: {comment.AuthorId}" +
                                  $"\nResponding to Post ID: {comment.RespondingToId}" +
                                  $"\nBody: \n{comment.Body}" +
                                  $"\nCreated on: {comment.DateCreated}");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task CreateCommentAsync()
        {
            Console.WriteLine("~~~~~~~~~~ Creating new comment ~~~~~~~~~~");

            Console.WriteLine("Enter the ID of the user creating the comment: ");
            string? readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int authorId))
            {
                Console.WriteLine("Author ID cannot be blank and must be a number.");
                return;
            }

            Console.WriteLine("Enter the ID of the post you are responding to: ");
            readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int respondingToId))
            {
                Console.WriteLine("Post ID cannot be blank and must be a number.");
                return;
            }

            Console.Write("Comment's text: ");
            string? body = Console.ReadLine();

            try
            {
                var newComment = await createCommentView.CreateComment(authorId, respondingToId, body);
                Console.WriteLine("~~~~~~~~~~ Successful comment creation ~~~~~~~~~~" +
                                  $"\nComment ID: {newComment.Id}" +
                                  $"\nBody: '{newComment.Body}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task UpdateCommentAsync()
        {
            Console.WriteLine("~~~~~~~~~~ Updating an existing comment ~~~~~~~~~~");
            await ListCommentsAsync();
            Console.Write("Enter the ID of the comment you want to update: ");
            string? readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id))
            {
                Console.WriteLine("ID cannot be blank and it must be a number.");
                return;
            }

            Comment comment;
            try
            {
                comment = await listCommentsView.GetComment(id);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            if (comment == null)
            {
                Console.WriteLine($"Comment with ID '{id}' does not exist.");
                return;
            }

            Console.Write("Enter new comment body: ");
            string? newBody = Console.ReadLine();
            try
            {
                await updateCommentView.UpdateComment(id, comment.RespondingToId, newBody);
                Console.WriteLine("~~~~~~~~~~ Successful comment update ~~~~~~~~~~" +
                                  $"\nOld Body: '{comment.Body}'" +
                                  $"\nNew Body: '{newBody}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task DeleteCommentAsync()
        {
            Console.WriteLine("~~~~~~~~~~ Deleting an existing comment ~~~~~~~~~~");
            await ListCommentsAsync();
            Console.Write("Enter the ID of the comment you want to delete: ");
            string? readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) || !int.TryParse(readLine, out int id))
            {
                Console.WriteLine("ID cannot be blank and it must be a number.");
                return;
            }

            try
            {
                var comment = await listCommentsView.GetComment(id);
                await updateCommentView.DeleteComment(id);
                Console.WriteLine("~~~~~~~~~~ Successful comment deletion ~~~~~~~~~~" +
                                  $"\nDeleted Body: '{comment.Body}'");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
