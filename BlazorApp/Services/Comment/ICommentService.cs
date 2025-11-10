using ApiContracts.Dtos.CommentDtos;
using ApiContracts.Dtos.PostDtos;

namespace BlazorApp.Services.Comment;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    public Task UpdateCommentAsync(int id, UpdateCommentDto request);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task<List<CommentDto>> GetCommentsAsync();
    public Task DeleteCommentAsync(int id);
}