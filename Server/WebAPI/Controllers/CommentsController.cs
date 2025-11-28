using ApiContracts.Dtos.CommentDtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        this.postRepo =  postRepo;
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }
    
    // CREATE
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto comment)
    {
        try
        {
            var user = await userRepo.GetSingleAsync(comment.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {comment.UserId} does not exist.");
            }

            var post = await postRepo.GetSingleAsync(comment.PostId);

            var toBeCreated = new Comment(comment.Body, comment.PostId, comment.UserId);
            toBeCreated.User = user;
            toBeCreated.Post = post;
            Comment created = await commentRepo.AddAsync(toBeCreated);
            
            CommentDto dto = new CommentDto()
            {
                Id = created.Id,
                PostId = created.PostId,
                UserId = created.UserId,
                Body = comment.Body,
            };
            return Created($"/comments/{dto.Id}", dto);
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
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            return Ok(new CommentDto()
            {
                Body = comment.Body,
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
            });
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
    public async Task<ActionResult<IEnumerable<Comment>>> GetMany(
        [FromQuery] int? postId,
        [FromQuery] int? userId,
        [FromQuery] string? userName)
    {
        try
        {
            IQueryable<Comment> comments = commentRepo.GetMany();

            // Filter by post ID
            if (postId.HasValue)
                comments = comments.Where(c => c.PostId == postId.Value);

            // Filter by user ID
            if (userId.HasValue)
                comments = comments.Where(c => c.UserId == userId.Value);

            // Filter by username
            if (!string.IsNullOrWhiteSpace(userName))
            {
                string lowered = userName.ToLower();

                // Get matching user IDs ASYNC
                var matchingUserIds = await userRepo.GetMany()
                    .Where(u => u.Username.ToLower().Contains(lowered))
                    .Select(u => u.Id)
                    .ToListAsync();

                // Apply filtering using SQL IN (...)
                comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
            }

            var result = await comments.ToListAsync();
            return Ok(result);
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
    [HttpGet("username/{id:int}")]
    public async Task<ActionResult<CommentUsername>> GetSingleWithComments(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            User? user = await userRepo.GetSingleAsync(comment.UserId);
            return Ok(new CommentUsername(comment.Id,comment.Body,user.Username, comment.PostId));
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