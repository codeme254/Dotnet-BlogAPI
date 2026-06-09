using BlogAPI.DTOs;
using BlogAPI.Models;
using BlogAPI.Repositories;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class AuthService(
    IUserRepository userRepository,
    IdGenerator idGen, IEmailService emailService,
    IVerificationTokenService verificationTokenService,
    IEmailTemplateService emailTemplateService,
    IConfiguration configuration
) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IdGenerator _idGen = idGen;
    private readonly IEmailService _emailService = emailService;
    private readonly IVerificationTokenService _verificationTokenService = verificationTokenService;
    private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
    private readonly IConfiguration _configuration = configuration;

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

        var verificationUrl = $"{_configuration["ClientURL"]}api/auth/verify-email?token={token}";
        var emailBody = _emailTemplateService.GetVerificationEmailBody(verificationUrl);

        await _emailService.SendEmailAsync(user.Email, "Welcome to Blog", emailBody);
    }
}