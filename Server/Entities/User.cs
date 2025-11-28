namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    // Navigation properties
    public List<Post> Posts { get; private set;  } = new();
    public List<Comment> Comments { get; private set; } = new();
    
    // EF Core requires a private empty constructor
    private User() {}

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}