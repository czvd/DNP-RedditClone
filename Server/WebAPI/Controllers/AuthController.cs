using ApiContracts.Dtos;
using ApiContracts.Dtos.AuthDtos;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = userRepo.GetMany().SingleOrDefault(u => u.Username == request.UserName);

            if (user == null)
                return Unauthorized("User not fount");
            
            if (user.Password != request.Password)
                return Unauthorized("Incorrect password");

            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };

            return Ok(dto);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}