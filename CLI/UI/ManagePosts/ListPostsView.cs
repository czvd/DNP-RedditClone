using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepo;

    public ListPostsView(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Posts (id, title) ===");
        Console.WriteLine();

        var posts = postRepo.GetManyAsync().ToList();

        if (posts.Count == 0)
        {
            Console.WriteLine("(no posts yet)");
        }
        else
        {
            foreach (var post in posts)
            {
                Console.WriteLine($"  {post.Id} {post.Title}");
            }
        }
        
        Console.WriteLine();
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}