using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public SinglePostView(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== View Post ===");

        // Ask for Post ID
        Console.Write("Post Id: ");
        string? input = Console.ReadLine();
        
        if (!int.TryParse(input, out int postId))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
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
        
        // Get Author's Username
        string authorName = "Unknown";
        try
        {
            var author = await userRepo.GetSingleAsync(post.UserId);
            authorName = author.Username;
        }
        catch
        {
            // keep "Unknown" if user is missing
        }
        
        Console.Clear();
        Console.WriteLine($"[{post.Id}] {post.Title}");
        Console.WriteLine($"By {authorName}");
        Console.WriteLine();
        Console.WriteLine(post.Body);
        Console.WriteLine();
        
        // Get comments for this post
        var comments = commentRepo.GetManyAsync()
            .Where(c => c.PostId == post.Id)
            .ToList();

        Console.WriteLine("Comments:");

        if (comments.Count == 0)
        {
            Console.WriteLine("(no comments yet)");
        }
        else
        {
            foreach (var comment in comments)
            {
                string commenterName = "Unknown";

                try
                {
                    var commenter = await userRepo.GetSingleAsync(comment.UserId);
                    commenterName = commenter.Username;
                }
                catch { }

                Console.WriteLine($"- ({comment.Id}) {commenterName}: {comment.Body}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}