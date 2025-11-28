using ApiContracts.Dtos.PostDtos;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    public Task UpdatePostAsync(int id, UpdatePostDto request);
    public Task<PostDto> GetPostAsync(int id);
    public Task<List<PostDto>> GetPostsAsync();
    public Task DeletePostAsync(int id);
}