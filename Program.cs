using BlogAPI.Data;
using BlogAPI.DTOs;
using BlogAPI.Middlewares;
using BlogAPI.Repositories;
using BlogAPI.Repositories.Implementations;
using BlogAPI.Services;
using BlogAPI.Services.Implementations;
using BlogAPI.Validators;
using FluentValidation;
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

// Services
builder.Services.AddScoped<IAuthService, AuthService>();

// Validators
builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "Welcome to BlogAPI");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
