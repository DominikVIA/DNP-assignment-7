using ApiContracts.Comments;
using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }
    
    // POST localhost:7065/Comments - creates a new comment
    [HttpPost]
    public async Task<IResult> CreateComment([FromBody] CreateCommentDto comment)
    {
        // verify that user with authorId exists
        
        Comment temp = new Comment(comment.AuthorId, comment.RespondingToId, comment.Body, DateTime.Now);
        Comment result = await commentRepo.AddAsync(temp);
        return Results.Created($"comments/{result.Id}", result);
    }
    
    // //GET https://localhost:7065/Comments/{id} - gets a single comment with given id
    // [HttpGet("{id:int}")]
    // public async Task<IResult> GetSingleComment([FromRoute] int id)
    // {
    //     try
    //     {
    //         Comment result = await commentRepo.GetSingleAsync(id);
    //         return Results.Ok(result);
    //     }
    //     catch (KeyNotFoundException e)
    //     {
    //         Console.WriteLine(e);
    //         return Results.NotFound(e.Message);
    //     }
    // }
    
    //GET https://localhost:7065/Comments/{id}?includeAuthor=true&includeParentContent=true
    // gets a single comment with given id and details
    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleComment(
        [FromServices] IUserRepository userRepo,
        [FromServices] IPostRepository postRepo,
        [FromRoute] int id,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeParentContent
        )
    {
        try
        {
            Comment temp = await commentRepo.GetSingleAsync(id);
            CommentDto result = new CommentDto()
            {
                Id = temp.Id,
                AuthorId = temp.AuthorId,
                Body = temp.Body,
                DateCreated = temp.DateCreated,
                RespondingToId = temp.RespondingToId
            };

            if (includeAuthor)
            {
                result.Author = await userRepo.GetSingleAsync(temp.AuthorId);
            }

            if (includeParentContent)
            {
                result.RespondingTo = await postRepo.GetSingleAsync(temp.RespondingToId);
            }
            
            return Results.Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    // GET https://localhost:7065/Comments - gets all comments
    [HttpGet]
    public IResult GetComments()
    {
        IQueryable<Comment> comments = commentRepo.GetMany();
        return Results.Ok(comments);
    }
    [HttpGet("ByUserId")]
    public IResult GetCommentsByUserId([FromQuery] int userId)
    {
        IQueryable<Comment> comments = commentRepo.GetMany()
            .Where(c => c.AuthorId == userId);

        return Results.Ok(comments);
    }

    [HttpGet("ByPostId")]
    public IResult GetCommentsByPostId([FromQuery] int postId)
    {
        IQueryable<Comment> comments = commentRepo.GetMany()
            .Where(c => c.RespondingToId == postId);

        return Results.Ok(comments);
    }
    [HttpGet("ByUserName")]
    public IResult GetCommentsByUserName([FromQuery] string username)
    {
        var users = userRepo.GetMany()
            .Where(u => u.Username.Contains(username))
            .ToList(); 

        var userIds = users.Select(u => u.Id);

        IQueryable<Comment> comments = commentRepo.GetMany()
            .Where(c => userIds.Contains(c.AuthorId));

        return Results.Ok(comments);
    }


    
    // PUT https://localhost:7065/Comments/{id}
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateComment([FromRoute] int id,
        [FromBody] UpdateCommentDto request)
    {
        Comment comment = new Comment (-1, -1, request.Body, DateTime.MinValue)
        {
            Id = id,
        };
        comment = await commentRepo.UpdateAsync(comment);
        return Results.Created($"comments/{comment.Id}", comment);
    }
    
    // DELETE https://localhost:7065/Comments/{id} - deletes a comment with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}