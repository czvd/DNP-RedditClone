using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }
    
    // CREATE
    [HttpPost]
    public async Task<ActionResult<Comment>> AddComment([FromBody] Comment comment)
    {
        try
        {
            Comment created = await commentRepo.AddAsync(comment);
            return Created($"/comments/{created.Id}", created);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] Comment comment)
    {
        try
        {
            if (id != comment.Id)
                return BadRequest("Comment ID mismatch.");

            await commentRepo.UpdateAsync(comment);
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
    public async Task<ActionResult<Comment>> GetSingle(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            return Ok(comment);
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
    
    // GET MANY
    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetMany(
        [FromQuery] int? postId,
        [FromQuery] int? userId,
        [FromQuery] string? userName)
    {
        try
        {
            IQueryable<Comment> comments = commentRepo.GetManyAsync();

            // Filter by post ID
            if (postId.HasValue)
                comments = comments.Where(c => c.PostId == postId.Value);

            // Filter by user ID
            if (userId.HasValue)
                comments = comments.Where(c => c.UserId == userId.Value);

            // Filter by username (requires lookup in userRepo)
            if (!string.IsNullOrWhiteSpace(userName))
            {
                var users = userRepo.GetManyAsync()
                    .Where(u => u.Username.Contains(userName, StringComparison.OrdinalIgnoreCase))
                    .Select(u => u.Id)
                    .ToList();

                comments = comments.Where(c => users.Contains(c.UserId));
            }

            return Ok(comments.ToList());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
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