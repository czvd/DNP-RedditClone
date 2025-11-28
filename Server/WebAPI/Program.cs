using EfcRepositories;
using FileRepositories;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using AppContext = EfcRepositories.AppContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();
builder.Services.AddDbContext<AppContext>(options =>
    options.UseSqlite("Data Source=../EfcRepositories/app.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (   app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <-- This enables Swagger UI page
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();