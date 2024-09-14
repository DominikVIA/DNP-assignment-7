using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class PostInfoVerification(IPostRepository postRepo)
{
    public Task<int> VerifyId(string? id)
    {
        if (id is null || id.Length == 0) throw new ArgumentException("ID cannot be blank.");
        if (!id.All(char.IsDigit)) throw new ArgumentException("ID must be a number.");
        int idParsed = int.Parse(id);
        if(!postRepo.GetMany().Any(p => p.Id == idParsed)) throw new ArgumentException("Post with this ID does not exist.");
        return Task.FromResult(idParsed);
    }
    
    public Task VerifyTitle(string? title)
    {
        if (title is null || title.Length == 0) throw new ArgumentException("Title cannot be blank.");
        return Task.CompletedTask;
    }
    
    public Task VerifyBody(string? body)
    {
        if (body is null || body.Length == 0) throw new ArgumentException("Body cannot be blank.");
        return Task.CompletedTask;
    }
}