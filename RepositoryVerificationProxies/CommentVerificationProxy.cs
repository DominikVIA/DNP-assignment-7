using Entities;
using RepositoryContracts;

namespace RepositoryVerificationProxies
{
    public class CommentVerificationProxy : ICommentRepository
    {
        private static ICommentRepository commentRepo;

        public CommentVerificationProxy(ICommentRepository commentRepo)
        {
            CommentVerificationProxy.commentRepo = commentRepo;
        }

        public static Task<int> VerifyCommentId(int? id)
        {
            if (id is null) throw new InvalidOperationException("ID cannot be blank.");
            if (id < 0) throw new InvalidOperationException("ID cannot be less than zero.");
            // if (!id.All(char.IsDigit)) throw new InvalidOperationException("ID must be a number.");
            if (!commentRepo.GetMany().Any(p => p.Id == id))
                throw new InvalidOperationException("Post with this ID does not exist.");
            return Task.FromResult((int)id);
        }


        private Task VerifyBody(string? body)
        {
            if (body is null || body.Length == 0 || body.Equals(" "))
                throw new InvalidOperationException("Body cannot be blank.");
            return Task.CompletedTask;
        }

        public Task<Comment> AddAsync(Comment comment)
        {
            UserVerificationProxy.VerifyUserId(comment.AuthorId);
            PostVerificationProxy.VerifyPostId(comment.RespondingToId);
            VerifyBody(comment.Body);
            return commentRepo.AddAsync(comment);
        }

        public Task UpdateAsync(Comment comment)
        {
            VerifyBody(comment.Body);
            return commentRepo.UpdateAsync(comment);
        }

        public Task DeleteAsync(int id)
        {
            return commentRepo.DeleteAsync(VerifyCommentId(id).Result);
        }

        public Task<Comment> GetSingleAsync(int id)
        {
            return commentRepo.GetSingleAsync(VerifyCommentId(id).Result);
        }

        public IQueryable<Comment> GetMany()
        {
            return commentRepo.GetMany();
        }
    }
}
