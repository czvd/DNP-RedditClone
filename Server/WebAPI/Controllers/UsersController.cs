using ApiContracts.Dtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            User user = new(request.UserName, request.Password);
            User created = await userRepo.AddAsync(user);
            
            UserDto dto = new()
            {
                Id = created.Id,
                Username = created.Username
            };
            
            return Created($"/api/users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User updatedUser = new(request.UserName, request.Password) { Id = id };
            await userRepo.UpdateAsync(updatedUser);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // GET SINGLE
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            UserDto dto = new()
            {
                Id = user.Id,
                Username = user.Username
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // GET MANY (with optional filtering by username substring)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMany([FromQuery] string? usernameContains)
    {
        try
        {
            var query = userRepo.GetMany();

            if (!string.IsNullOrWhiteSpace(usernameContains))
            {
                query = query.Where(u => u.Username.Contains(usernameContains, StringComparison.OrdinalIgnoreCase));
            }

            var result = await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToListAsync();

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}