using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }
    
    // POST localhost:7065/Posts - creates a new post
    [HttpPost]
    public async Task<IResult> CreatePost([FromBody] CreatePostDto post)
    {
        // verify that user with authorId exists
        
        Post temp = new Post(post.AuthorId, post.Title, post.Body, DateTime.Now);
        Post result = await postRepo.AddAsync(temp);
        return Results.Created($"posts/{result.Id}", result);
    }
    
    [HttpGet("ByAuthor")]
    public async Task<IResult> GetPostsByAuthor(
        [FromQuery] int? authorId,
        [FromQuery] string? authorName)
    {
        IQueryable<Post> posts = postRepo.GetMany();

        if (authorId.HasValue)
        {
            posts = posts.Where(p => p.AuthorId == authorId.Value);
        }

        if (!string.IsNullOrEmpty(authorName))
        {
            var users = userRepo.GetMany()
                .Where(u => u.Username.Contains(authorName))
                .ToList();

            var userIds = users.Select(u => u.Id);
            posts = posts.Where(p => userIds.Contains(p.AuthorId));
        }

        return Results.Ok(posts);
    }

    
    //GET https://localhost:7065/Posts/{id}?includeComment = true&includeAuthor = true&includeReactions = true
    // gets a single post with given id and with given details
    [HttpGet("{id:int}")]
    public async Task<IResult> GetPostWithChoices(
        [FromServices] IUserRepository userRepo,
        [FromServices] ICommentRepository commentRepo,
        [FromServices] IReactionRepository reactionRepo,
        [FromRoute] int id,
        [FromQuery] bool includeComments,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeReactions
        )
    {
        try
        {
            Post temp = await postRepo.GetSingleAsync(id);
            PostDto gotten = new()
            {
                Id = temp.Id,
                authorId = temp.AuthorId,
                title = temp.Title,
                body = temp.Body,
                dateCreated = temp.DateCreated
            };
            
            if (includeComments)
            {
                gotten.comments = commentRepo.GetMany()
                    .Where(comment => comment.RespondingToId == temp.Id)
                    .ToList();
            }
            
            if (includeAuthor)
            {
                gotten.author = await userRepo.GetSingleAsync(temp.AuthorId);
            }
            
            if (includeReactions)
            {
                IQueryable<Reaction> reactions = reactionRepo.GetMany()
                    .Where(reaction => reaction.ContentId == temp.Id);
                
                gotten.likes = reactions.Count(reaction => reaction.Like);
                gotten.dislikes = reactions.Count(reaction => !reaction.Like);
            }
            
            return Results.Ok(gotten);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    // GET https://localhost:7065/Posts - gets all comments
    [HttpGet]
    public IResult GetPosts()
    {
        IQueryable<Post> posts = postRepo.GetMany();
        return Results.Ok(posts);
    }
    
    [HttpGet("ByTitle")]
    public IResult GetPostsByTitle([FromQuery] string titleContains)
    {
        IQueryable<Post> posts = postRepo.GetMany()
            .Where(p => p.Title.Contains(titleContains));

        return Results.Ok(posts);
    }


    // PUT https://localhost:7065/Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdatePost([FromRoute] int id,
        [FromBody] UpdatePostDto request)
    {
        Post post = new Post (-1, request.Title, request.Body, DateTime.MinValue)
        {
            Id = id,
        };
        post = await postRepo.UpdateAsync(post);
        return Results.Created($"posts/{post.Id}", post);
    }
    
    // DELETE https://localhost:7065/Posts/{id} - deletes a post with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}