using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CreatePostView(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Create Post ===");

        var users = userRepo.GetManyAsync().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("Cannot create post — no users exist in the system.");
            Console.WriteLine("Please create a user first.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return; 
        }
        
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.WriteLine("Body: ");
        string body = Console.ReadLine() ?? "";
        
        Console.WriteLine("User ID: ");
        int userId;
        
        while (!int.TryParse(Console.ReadLine(), out userId))
        {
            Console.Write("Invalid input. Please enter a valid number for User ID: ");
        }

        Post newPost = new()
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post createdPost = await postRepo.AddAsync(newPost);

        Console.WriteLine();
        Console.WriteLine($"Created Post {createdPost.Id}.");
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}