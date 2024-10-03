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
    
    //GET https://localhost:7065/Posts/{id} - gets a single post with given id
    [HttpGet("{id:int}")]
    public async Task<IResult> GetSinglePost([FromRoute] int id)
    {
        try
        {
            Post result = await postRepo.GetSingleAsync(id);
            return Results.Ok(result);
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