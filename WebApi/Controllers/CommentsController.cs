using ApiContracts.Comments;
using ApiContracts.Content;
using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        Comment temp = new Comment
        {
            AuthorId = comment.AuthorId, 
            RespondingToId = comment.RespondingToId, 
            Body = comment.Body, 
            DateCreated = DateTime.Now
        };
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
        [FromRoute] int id,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeParentContent,
        [FromQuery] bool includeComments)
    {
        var queryForComment = commentRepo.GetMany().Where(c => c.Id == id).AsQueryable();

        if (includeAuthor)
            queryForComment.Include(c => c.Author);
        
        if (includeParentContent)
            queryForComment.Include(c => c.RespondingTo);
        
        if (includeComments)
            queryForComment.Include(c => c.Comments);

        CommentDto? dto = await queryForComment.Select(c => new CommentDto
            {
                Id = c.Id,
                AuthorId = c.AuthorId,
                Body = c.Body,
                DateCreated = c.DateCreated,
                RespondingToId = c.RespondingToId,

                RespondingTo = includeParentContent ?
                    new ContentDto
                    {
                        Id = c.RespondingTo.Id,
                        AuthorId = c.RespondingTo.AuthorId,
                        Body = c.RespondingTo.Body,
                        DateCreated = c.RespondingTo.DateCreated,
                    } : null,
                Author = includeAuthor
                    ? new UserDto
                    {
                        Id = c.Author.Id,
                        Username = c.Author.Username
                    }
                    : null,
                Comments = includeComments
                    ? c.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Body = c.Body,
                        AuthorId = c.AuthorId,
                        DateCreated = c.DateCreated
                    }).ToList()
                    : new(),
            }
        ).FirstOrDefaultAsync();
        return Results.Ok(dto);
    }

    
    // GET https://localhost:7065/Comments - gets all comments
    [HttpGet]
    public IResult GetComments()
    {
        IQueryable<CommentDto> comments = commentRepo.GetMany().Select(c=> new CommentDto()
        {
            Id = c.Id,
            AuthorId = c.AuthorId,
            RespondingToId = c.RespondingToId,
            Body = c.Body,
            DateCreated = c.DateCreated,
        });
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
        try
        {
            // Actualizamos el comentario
            Comment? requestedComment = await commentRepo.GetSingleAsync(id);
            if(requestedComment == null) throw new KeyNotFoundException("Comment with id: " + id + " not found");
            Comment comment = new Comment { 
                Id = requestedComment.Id, 
                AuthorId = requestedComment.AuthorId, 
                Body = request.Body, 
                DateCreated = requestedComment.DateCreated 
            };
            await commentRepo.UpdateAsync(comment);

            // Recuperamos el comentario actualizado
            Comment? updatedComment = await commentRepo.GetSingleAsync(comment.Id);
            if (updatedComment == null)
            {
                return Results.NotFound($"Comment with id {id} not found after update");
            }

            return Results.Ok(updatedComment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }

    
    // DELETE https://localhost:7065/Comments/{id} - deletes a comment with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}