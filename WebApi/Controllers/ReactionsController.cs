using ApiContracts.Content;
using ApiContracts.Reactions;
using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReactionsController
{
        private readonly IReactionRepository reactionRepo;

    public ReactionsController(IReactionRepository reactionRepo)
    {
        this.reactionRepo = reactionRepo;
    }
    
    // POST localhost:7065/Reactions - creates a new reaction
    [HttpPost]
    public async Task<IResult> CreateReaction([FromBody] CreateReactionDto reaction)
    {
        // verify that user with authorId exists
        
        Reaction temp = new Reaction
        {
            UserId = reaction.UserId, 
            ContentId = reaction.ContentId, 
            Like = reaction.Like, 
            DateCreated = DateTime.Now
        };
        Reaction result = await reactionRepo.AddAsync(temp);
        return Results.Created($"reactions/{result.UserId}/{result.ContentId}", result);
    }
    
    // //GET https://localhost:7065/Reactions/{id} - gets a single reaction with given id
    // [HttpGet("{id:int}")]
    // public async Task<IResult> GetSingleReaction([FromRoute] int id)
    // {
    //     try
    //     {
    //         Reaction result = await reactionRepo.GetSingleAsync(id);
    //         return Results.Ok(result);
    //     }
    //     catch (KeyNotFoundException e)
    //     {
    //         Console.WriteLine(e);
    //         return Results.NotFound(e.Message);
    //     }
    // }
    
    //GET https://localhost:7065/Reactions/{id} - gets a single reaction with given id
    [HttpGet("{userId:int}/{contentId:int}")]
    public async Task<IResult> GetSingleReaction(
        [FromRoute] int userId,
        [FromRoute] int contentId,
        [FromQuery] bool includeUser,
        [FromQuery] bool includeContent
        )
    {
        var queryForReaction = reactionRepo.GetMany()
            .Where(r => r.UserId == userId && r.ContentId == contentId)
            .AsQueryable();

        if (includeUser)
            queryForReaction.Include(r => r.User);
        
        if (includeContent)
            queryForReaction.Include(r => r.Content);
        
        ReactionDto? dto = await queryForReaction.Select(r => new ReactionDto
        {
            UserId = r.UserId,
            ContentId = r.ContentId,
            Like = r.Like,
            DateCreated = r.DateCreated,
            
            Author = includeUser ? new UserDto
            {
                Id = r.User.Id,
                Username = r.User.Username,
            } : null,
            
            Content = includeContent ? new ContentDto
            {
                Id = r.Content.Id,
                AuthorId = r.Content.AuthorId,
                Body = r.Content.Body,
                DateCreated = r.Content.DateCreated
            } : null
            
        }).FirstOrDefaultAsync();
        return Results.Ok(dto);
    }
    
    // GET https://localhost:7065/Reactions - gets all reactions
    [HttpGet]
    public IResult GetReactions()
    {
        IQueryable<Reaction> reactions = reactionRepo.GetMany();
        return Results.Ok(reactions);
    }
    
    // // PUT https://localhost:7065/Reactions/{id}
    // [HttpPut("{id:int}")]
    // public async Task<IResult> UpdateReaction([FromRoute] int id,
    //     [FromBody] UpdateReactionDto request)
    // {
    //     Reaction reaction = new Reaction (-1, -1, request.Like, DateTime.MinValue)
    //     {
    //         UserId = -1,
    //         ContentId = -1,
    //         request.Like,
    //     };
    //     reaction = await reactionRepo.UpdateAsync(reaction);
    //     return Results.Created($"reactions/{reaction.Id}", reaction);
    // }
    
    // DELETE https://localhost:7065/Reactions/{id} - deletes a reaction with a given id
    [HttpDelete("{userId:int}/{contentId:int}")]
    public async Task<IResult> DeleteReaction(
        [FromRoute] int userId,
        [FromRoute] int contentId)
    {
        await reactionRepo.DeleteAsync(userId, contentId);
        return Results.NoContent();
    }
}