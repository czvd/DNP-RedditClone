namespace Entities;

public class Comment
{ 
    public int Id { get; set; }
    public string Body { get; set; } = "";
    
    // Foreign keys
    public int PostId { get; set; }
    public int UserId { get; set; }
    
    // Navigation properties
    public Post? Post { get; set; } = null!;
    public User User { get; set; }
    
    private Comment() {} // Required by EF Core

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}