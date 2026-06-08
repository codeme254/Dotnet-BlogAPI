using BlogAPI.Data;
using IdGen;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure the Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// IdGen Service
builder.Services.AddSingleton(_ => new IdGenerator(0));

builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "Welcome to BlogAPI");
app.MapControllers();

app.Run();
