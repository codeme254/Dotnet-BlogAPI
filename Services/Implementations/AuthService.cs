using BlogAPI.DTOs;
using BlogAPI.Models;
using BlogAPI.Repositories;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class AuthService(IUserRepository userRepository, IdGenerator idGen) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
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

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }
}