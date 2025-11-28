using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class AddCommentView
{
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public AddCommentView(IPostRepository postRepo, ICommentRepository commentRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add Comment ===");
        Console.WriteLine();
        
        // Ask for Post ID
        Console.Write("Post Id: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid input. Please enter a valid number for Post Id.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return;
        }
        
        // Validate post exists
        Post post;
        try
        {
            post = await postRepo.GetSingleAsync(postId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return;
        }
        
        // Ask for User ID
        Console.Write("User Id: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid input. Please enter a valid number for User Id.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return;
        }

        // Validate user exists
        User user;
        try
        {
            user = await userRepo.GetSingleAsync(userId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"User with ID {userId} not found.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return;
        }
        
        // Ask for comment text
        Console.Write("Comment: ");
        string body = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            return;
        }

        Comment newComment = new(body, post.Id, user.Id);
        
        Comment createdComment = await commentRepo.AddAsync(newComment);

        Console.WriteLine();
        Console.WriteLine($"Added comment {createdComment.Id}.");
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}