using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Create User ===");
        
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        Console.WriteLine("Password: ");
        string password = Console.ReadLine() ?? "";

        User newUser = new User(username, password);

        User createdUser = await userRepo.AddAsync(newUser);

        Console.WriteLine($"Created user with Id {createdUser.Id}");
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}