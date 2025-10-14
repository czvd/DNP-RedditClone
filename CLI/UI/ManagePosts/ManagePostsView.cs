using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public ManagePostsView(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    public async Task RunAsync()
    { 
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Posts");
            Console.WriteLine("1) Create post");
            Console.WriteLine("2) View post");
            Console.WriteLine("3) List all posts");
            Console.WriteLine("4) Add comment to a post");
            Console.WriteLine("0) Back");
            Console.WriteLine("Select: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await new CreatePostView(postRepo, userRepo).RunAsync();
                    break;
                case "2":
                    await new SinglePostView(postRepo, userRepo, commentRepo).RunAsync();
                    break;
                case "3":
                    await new ListPostsView(postRepo).RunAsync();
                    break;
                case "4":
                    await new AddCommentView(postRepo, commentRepo, userRepo).RunAsync();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press ENTER to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }
}