namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    
    // Foreign key
    public int UserId { get; set; }
    
    // Navigation to User
    public User? User { get; set; }
    
    // Navigation to comments
    public List<Comment> Comments { get; private set; } = new();
    
    private Post() {}
    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }
}