using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }
    
    // CREATE
    [HttpPost]
    public async Task<ActionResult<Post>> AddPost([FromBody] Post post)
    {
        try
        {
            Post created = await postRepo.AddAsync(post);
            return Created($"/posts/{created.Id}", created);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] Post post)
    {
        try
        {
            if (id != post.Id)
                return BadRequest("Post ID mismatch.");

            await postRepo.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // GET SINGLE
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetSingle(int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            return Ok(post);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // GET MANY (with filters)
    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetMany([FromQuery] string? title, [FromQuery] int? userId)
    {
        try
        {
            IQueryable<Post> posts = postRepo.GetManyAsync();

            if (!string.IsNullOrWhiteSpace(title))
                posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (userId.HasValue)
                posts = posts.Where(p => p.UserId == userId.Value);

            return Ok(posts.ToList());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}