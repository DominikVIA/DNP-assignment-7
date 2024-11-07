using ApiContracts.Users;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = _userRepository.GetMany().ToList()
                .SingleOrDefault(u => u.Username == request.UserName); 

            if (user == null || user.Password != request.Password) 
            {
                return Unauthorized("Invalid username or password");
            }

            var userDto = new SimpleUserDto
            {
                Id = user.Id,
                UserName = user.Username,
            };

            return Ok(userDto);
        }
    }
}