using ApiContracts.Comments;
using ApiContracts.Posts;
using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        Post temp = new Post
        {
            AuthorId = post.AuthorId, 
            Title = post.Title, 
            Body = post.Body, 
            DateCreated = DateTime.Now
        };
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
        [FromRoute] int id,
        [FromQuery] bool includeComments,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeReactions)
    {
        IQueryable<Post> queryForPost = postRepo
            .GetMany()
            .Where(p => p.Id == id)
            .AsQueryable();

        if (includeAuthor)
        {
            queryForPost = queryForPost.Include(p => p.Author);
        }

        if (includeComments)
        {
            queryForPost = queryForPost.Include(p => p.Comments);
        }
        
        if (includeReactions)
        {
            queryForPost = queryForPost.Include(p => p.Reactions);
        }
        
        PostDto? dto = await queryForPost.Select(post => new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                AuthorId = post.AuthorId,
                DateCreated = post.DateCreated,
                Author = includeAuthor
                    ? new UserDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username
                    }
                    : null,
                Comments = includeComments
                    ? post.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Body = c.Body,
                        AuthorId= c.AuthorId,
                        DateCreated = c.DateCreated
                    }).ToList()
                    : new (),
                Likes = includeReactions ? post.Reactions
                    .Count(r => r.Like) : -1,
                Dislikes = includeReactions ? post.Reactions
                    .Count(r => !r.Like) : -1
            })
            .FirstOrDefaultAsync();

        return dto == null ? Results.NotFound() : Results.Ok(dto);
    }

    
    // GET https://localhost:7065/Posts - gets all comments
    [HttpGet]
    public async Task<IResult> GetPosts(        
        [FromQuery] bool includeComments,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeReactions)
    {
        List<PostDto> toReturn = new List<PostDto>();
        
        IQueryable<Post> queryForPosts = postRepo
            .GetMany()
            .AsQueryable();

        if (includeAuthor)
        {
            queryForPosts = queryForPosts.Include(p => p.Author);
        }

        if (includeComments)
        {
            queryForPosts = queryForPosts.Include(p => p.Comments).ThenInclude(c => c.Author);
        }
        
        if (includeReactions)
        {
            queryForPosts = queryForPosts.Include(p => p.Reactions);
        }
        
        foreach (var tempPost in queryForPosts)
        {
            PostDto? dto = new PostDto()
            {
                Id = tempPost.Id,
                Title = tempPost.Title,
                Body = tempPost.Body,
                AuthorId = tempPost.AuthorId,
                DateCreated = tempPost.DateCreated,
                Author = includeAuthor
                    ? new UserDto
                    {
                        Id = tempPost.Author.Id,
                        Username = tempPost.Author.Username
                    }
                    : null,
                Comments = includeComments
                    ? tempPost.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Body = c.Body,
                        AuthorId = c.AuthorId,
                        DateCreated = c.DateCreated,
                        Author = new UserDto
                        {
                            Id = c.Author.Id,
                            Username = c.Author.Username
                        }
                    }).ToList()
                    : new(),
                Likes = includeReactions ? tempPost.Reactions
                .Count(r => r.Like) : -1,
                Dislikes = includeReactions ? tempPost.Reactions
                    .Count(r => !r.Like) : -1
            };

            toReturn.Add(dto);
        }

        return Results.Ok(toReturn.AsQueryable());
    }
    
    [HttpGet("ByTitle")]
    public async Task<IResult> GetPostsByTitle([FromQuery] string titleContains)
    {
        List<PostDto> posts = await postRepo.GetMany()
            .Where(p => p.Title.Contains(titleContains))
            .Select(p => new PostDto()
            {
                Id = p.Id,
                Title = p.Title,
                AuthorId = p.AuthorId,
                Body = p.Body,
                DateCreated = p.DateCreated
            })
            .ToListAsync();

        return Results.Ok(posts);
    }


    // PUT https://localhost:7065/Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdatePost([FromRoute] int id,
        [FromBody] UpdatePostDto request)
    {
        try
        {
            Post? requestedPost = await postRepo.GetSingleAsync(id);
            if (requestedPost == null) throw new KeyNotFoundException($"Post with id {id} not found");
            Post post = new Post
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                DateCreated = requestedPost.DateCreated
            };

            await postRepo.UpdateAsync(post); // No return value
            return Results.Ok($"Post with id {id} updated successfully");
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }

    
    // DELETE https://localhost:7065/Posts/{id} - deletes a post with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}