using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;

    public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        this.userRepo = userRepo;
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
    }

    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Simple Blog CLI ===");
            Console.WriteLine();
            Console.WriteLine("Main Menu");
            Console.WriteLine("1) Manage Users");
            Console.WriteLine("2) Manage Posts & Comments");
            Console.WriteLine("0) Exit");
            Console.Write("Select: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var manageUsersView = new ManageUsersView(userRepo);
                    await manageUsersView.RunAsync();
                    break;
                case "2":
                    var managePostsView = new ManagePostsView(postRepo, userRepo, commentRepo);
                    await managePostsView.RunAsync();
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
        Console.Clear();
        Console.WriteLine("Goodbye!");
    }
}