using BlogAPI.DTOs;
using BlogAPI.Models;
using BlogAPI.Repositories;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class AuthService(
    IUserRepository userRepository,
    IdGenerator idGen, IEmailService emailService,
    IVerificationTokenService verificationTokenService
) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IdGenerator _idGen = idGen;
    private readonly IEmailService _emailService = emailService;
    private readonly IVerificationTokenService _verificationTokenService = verificationTokenService;

    public async Task RegisterAsync(RegisterDTO registerDTO)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
        var user = new User
        {
            UserId = _idGen.CreateId(),
            Username = registerDTO.Username,
            Email = registerDTO.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = await _verificationTokenService.CreateVerificationToken(user.Email);

        await _emailService.SendEmailAsync(user.Email, "Welcome to Blog",
        $"""
        <h1>Welcome to Blog.</h1>

        You are going to like it here.

        <p>Your verification token is <em>{token}</em></p>
        """);
    }
}