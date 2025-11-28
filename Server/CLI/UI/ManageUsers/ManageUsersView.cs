using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepo;

    public ManageUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task RunAsync()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Users");
            Console.WriteLine("1) Create user");
            Console.WriteLine("2) List users");
            Console.WriteLine("0) Back");
            Console.WriteLine("Select: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await new CreateUserView(userRepo).RunAsync();
                    break;
                case "2":
                    await new ListUsersView(userRepo).RunAsync();
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