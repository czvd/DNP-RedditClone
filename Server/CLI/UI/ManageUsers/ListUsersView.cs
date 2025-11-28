using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepo;

    public ListUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Users (id, username) ===");
        Console.WriteLine();

        var users = userRepo.GetMany().ToList();

        if (users.Count == 0)
        {
            Console.WriteLine("(no users yet)");
        }
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine($"  {user.Id} {user.Username}");
            }
        }
        
        Console.WriteLine();
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}