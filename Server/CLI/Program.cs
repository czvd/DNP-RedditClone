using CLI.UI;
using Entities;
using FileRepositories;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();

/*await userRepository.AddAsync(new User { Username = "alice", Password = "pw1" });
await userRepository.AddAsync(new User { Username = "bob", Password = "pw2" });

await postRepository.AddAsync(new Post { Title = "Hello world", Body = "My first post!", UserId = 1 });
await postRepository.AddAsync(new Post { Title = "Second post", Body = "Another day, another post.", UserId = 2 });

await commentRepository.AddAsync(new Comment { Body = "Nice post!", UserId = 2, PostId = 1 });
await commentRepository.AddAsync(new Comment { Body = "Thanks for sharing!", UserId = 1, PostId = 2 });*/

CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository);
await cliApp.StartAsync();
