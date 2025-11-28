using ApiContracts.Dtos.CommentDtos;
using ApiContracts.Dtos.PostDtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] Post post)
    {
        try
        {
            var user = await userRepo.GetSingleAsync(post.UserId);
            if (user == null)
            {
                return BadRequest($"User with ID {post.UserId} does not exist.");
            }

            post.User = user;
            Post created = await postRepo.AddAsync(post);
            PostDto dto = new PostDto()
            {
                Body = created.Body,
                Id = created.Id,
                Title = created.Title,
                UserId = created.UserId
            };
            return Created($"/posts/{created.Id}", dto);
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
    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            return Ok(new PostDto(){
                Body = post.Body,
                Title = post.Title,
                Id = post.Id,
                UserId = post.UserId
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

    // GET MANY (with filters)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetMany([FromQuery] string? title, [FromQuery] int? userId)
    {
        try
        {
            IQueryable<Post> posts = postRepo.GetMany();

            if (!string.IsNullOrWhiteSpace(title))
                posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (userId.HasValue)
                posts = posts.Where(p => p.UserId == userId.Value);
            var sortedPosts = await posts.ToListAsync();
            List<PostDto> postDtos = new List<PostDto>();
            foreach (var post in sortedPosts)
            {
                postDtos.Add(new PostDto(){
                Body = post.Body,
                Title = post.Title,
                Id = post.Id,
                UserId = post.UserId
                });
            }

            return Ok(postDtos);
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
    [HttpGet("withComments/{postId}")]
    public async Task<ActionResult<PostWithCommentsDto>> GetSinglePost(
        int postId)
    {
        var post = await postRepo.GetSingleAsync(postId);

        var postComments = await commentRepo
            .GetMany()
            .Where(c => c.PostId == postId)
            .ToListAsync();

        var userIds = postComments.Select(c => c.UserId).Append(post.UserId)
            .Distinct();
        var users = await userRepo
            .GetMany()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        var commentDtos = postComments.Select(c =>
        {
            var user = users.Single(u => u.Id == c.UserId);
            return new CommentUsername(c.Id, c.Body, user.Username, c.PostId);
        }).ToList();

        var postWriter = users.Single(u => u.Id == post.UserId);
        var dto = new PostWithCommentsDto(
            new PostWithUsername(post.Id, post.Title, post.Body, postWriter.Username),
            commentDtos
        );
        return Ok(dto);
    }
}