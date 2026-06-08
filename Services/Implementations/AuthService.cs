using BlogAPI.Data;
using BlogAPI.DTOs;
using BlogAPI.Models;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class AuthService(AppDbContext dbContext, IdGenerator idGen) : IAuthService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IdGenerator _idGen = idGen;

    public async Task RegisterAsync(RegisterDTO registerDTO)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
        var user = new User
        {
            UserId = _idGen.CreateId(),
            Username = registerDTO.Username,
            Email = registerDTO.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}