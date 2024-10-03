using ApiContracts.Reactions;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
        
        Reaction temp = new Reaction(reaction.UserId, reaction.ContentId, reaction.Like, DateTime.Now);
        Reaction result = await reactionRepo.AddAsync(temp);
        return Results.Created($"reactions/{result.Id}", result);
    }
    
    //GET https://localhost:7065/Reactions/{id} - gets a single reaction with given id
    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleReaction([FromRoute] int id)
    {
        try
        {
            Reaction result = await reactionRepo.GetSingleAsync(id);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    // GET https://localhost:7065/Reactions - gets all reactions
    [HttpGet]
    public IResult GetReactions()
    {
        IQueryable<Reaction> reactions = reactionRepo.GetMany();
        return Results.Ok(reactions);
    }
    
    // PUT https://localhost:7065/Reactions/{id}
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateReaction([FromRoute] int id,
        [FromBody] UpdateReactionDto request)
    {
        Reaction reaction = new Reaction (-1, -1, request.Like, DateTime.MinValue)
        {
            Id = id,
        };
        reaction = await reactionRepo.UpdateAsync(reaction);
        return Results.Created($"reactions/{reaction.Id}", reaction);
    }
    
    // DELETE https://localhost:7065/Reactions/{id} - deletes a reaction with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteReaction([FromRoute] int id)
    {
        await reactionRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}