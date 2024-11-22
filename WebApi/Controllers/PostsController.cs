using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts.Comments;
using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Http;
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
        [FromServices] IUserRepository userRepo,
        [FromServices] ICommentRepository commentRepo,
        [FromServices] IReactionRepository reactionRepo,
        [FromRoute] int id,
        [FromQuery] bool includeComments,
        [FromQuery] bool includeAuthor,
        [FromQuery] bool includeReactions)
    {
        Post? temp = null;
        try
        {
            // Simulate getting the Post, as the method does not return the entity
            await postRepo.GetSingleAsync(id);
            temp = postRepo.GetMany().FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return Results.NotFound($"Post with id {id} not found");
            }

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
                gotten.comments = [];
                List<Comment> comments = commentRepo.GetMany()
                    .Where(comment => comment.RespondingToId == temp.Id)
                    .ToList();
                foreach (var comment in comments)
                {
                    gotten.comments.Add(new CommentDto
                    {
                        Id = comment.Id,
                        AuthorId = comment.AuthorId,
                        Body = comment.Body,
                        DateCreated = comment.DateCreated
                    });
                }
            }

            if (includeAuthor)
            {
                await userRepo.GetSingleAsync(temp.AuthorId);

                gotten.author = userRepo.GetMany().FirstOrDefault(u => u.Id == temp.AuthorId);
                if (gotten.author == null)
                {
                    return Results.NotFound($"Author with id {temp.AuthorId} not found");
                }
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
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }

    
    // GET https://localhost:7065/Posts - gets all comments
    [HttpGet]
    public async Task<IResult> GetPosts(
        [FromServices] IUserRepository userRepo,
        [FromServices] ICommentRepository commentRepo,
        [FromServices] IReactionRepository reactionRepo)
    {
        IQueryable<Post> posts = postRepo.GetMany();
        List<PostDto> toReturn = new List<PostDto>();
        foreach (var tempPost in posts)
        {
            PostDto gotten = new()
            {
                Id = tempPost.Id,
                authorId = tempPost.AuthorId,
                title = tempPost.Title,
                body = tempPost.Body,
                dateCreated = tempPost.DateCreated
            };

            IQueryable<Comment> comments = commentRepo.GetMany()
                .Where(comment => comment.RespondingToId == tempPost.Id);

            gotten.comments = new List<CommentDto>();

            foreach (var comment in comments)
            {
                CommentDto tempCommentDto = new CommentDto {
                    Id = comment.Id, 
                    AuthorId = comment.AuthorId, 
                    Body = comment.Body, 
                    DateCreated = comment.DateCreated
                };

                await userRepo.GetSingleAsync(comment.AuthorId);
                tempCommentDto.Author = userRepo.GetMany().FirstOrDefault(u => u.Id == comment.AuthorId);
                if (tempCommentDto.Author == null)
                {
                    return Results.NotFound($"Author with id {comment.AuthorId} not found");
                }

                gotten.comments.Add(tempCommentDto);
            }

            await userRepo.GetSingleAsync(tempPost.AuthorId);
            gotten.author = userRepo.GetMany().FirstOrDefault(u => u.Id == tempPost.AuthorId);
            if (gotten.author == null)
            {
                return Results.NotFound($"Author with id {tempPost.AuthorId} not found");
            }

            toReturn.Add(gotten);
        }

        return Results.Ok(toReturn.AsQueryable());
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