using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    // POST localhost:7065/Users - creates a new user
    [HttpPost]
    public async Task<IResult> CreateUser([FromBody] CreateUserDto user)
    {
        User temp = new User
        {
            Username = user.Username, 
            Password = user.Password
        };
        User result = await userRepo.AddAsync(temp);
        return Results.Created($"users/{result.Id}", result);
    }
    
    // //GET https://localhost:7065/Users/{id} - gets a single user with id
    // [HttpGet("{id:int}")]
    // public async Task<IResult> GetSingleUser([FromRoute] int id)
    // {
    //     try
    //     {
    //         User result = await userRepo.GetSingleAsync(id);
    //         return Results.Ok(result);
    //     }
    //     catch (KeyNotFoundException e)
    //     {
    //         Console.WriteLine(e);
    //         return Results.NotFound(e.Message);
    //     }
    // }
    
        //GET https://localhost:7065/Users/{id}?includePosts=true&includeComments=true
        // gets a single user with id and with details
        [HttpGet("{id:int}")]
        public async Task<IResult> GetSingleUser(
            [FromServices] ICommentRepository commentRepo,
            [FromServices] IPostRepository postRepo,
            [FromRoute] int id,
            [FromQuery] bool includePosts,
            [FromQuery] bool includeComments)
        {
            try
            {
                await userRepo.GetSingleAsync(id);
        
                User? temp = userRepo.GetMany().FirstOrDefault(u => u.Id == id);
                if (temp == null)
                {
                    return Results.NotFound($"User with id {id} not found");
                }

                UserDto result = new UserDto()
                {
                    Id = temp.Id,
                    Username = temp.Username,
                    Password = temp.Password
                };

                if (includePosts)
                {
                    result.Posts = postRepo.GetMany()
                        .Where(p => p.AuthorId == result.Id)
                        .ToList();
                }

                if (includeComments)
                {
                    result.Comments = commentRepo.GetMany()
                        .Where(c => c.AuthorId == result.Id)
                        .ToList();
                }

                return Results.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem(e.Message);
            }
        }

    
    // GET https://localhost:7065/Users - gets all users
    [HttpGet]
    public async Task<IResult> GetUsers()
    {
        IQueryable<User> users = userRepo.GetMany();
        return Results.Ok(users);
    }
    
    // PUT https://localhost:7065/Users/{id}
    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateUser([FromRoute] int id,
        [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = new User
            {
                Username = request.Username,
                Password = request.Password,
                Id = id,
            };
            await userRepo.UpdateAsync(user);

            User? updatedUser = userRepo.GetMany().FirstOrDefault(u => u.Id == id);
            if (updatedUser == null)
            {
                return Results.NotFound($"User with id {id} not found after update");
            }

            return Results.Ok(updatedUser);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }

    
    // DELETE https://localhost:7065/Users/{id} - deletes a user with a given id
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteUser([FromRoute] int id)
    {
        await userRepo.DeleteAsync(id);
        return Results.NoContent();
    }
}