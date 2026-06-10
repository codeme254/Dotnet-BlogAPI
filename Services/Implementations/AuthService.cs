using BlogAPI.DTOs;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using BlogAPI.Repositories;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class AuthService(
    IUserRepository userRepository,
    IdGenerator idGen, IEmailService emailService,
    IVerificationTokenService verificationTokenService,
    IEmailTemplateService emailTemplateService,
    IConfiguration configuration,
    IVerificationTokenRepository verificationTokenRepository
) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IdGenerator _idGen = idGen;
    private readonly IEmailService _emailService = emailService;
    private readonly IVerificationTokenService _verificationTokenService = verificationTokenService;
    private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IVerificationTokenRepository _verificationTokenRepository = verificationTokenRepository;

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

    public async Task VerifyEmailAsync(string token)
    {
        var verificationToken = await _verificationTokenRepository.GetVerificationTokenAsync(token)
        ?? throw new TokenNotFoundException("Verification token not found");

        if (verificationToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new VerificationTokenExpiredException("Verification token has expired.");
        }

        var user = await _userRepository.GetUserAsync(verificationToken.Email)
        ?? throw new UserNotFoundException("User not found");

        user.IsVerified = true;
        _verificationTokenRepository.DeleteVerificationToken(verificationToken);
        await _userRepository.SaveChangesAsync();
        await _verificationTokenRepository.SaveChangesAsync();
    }

    public async Task ResendVerificationTokenAsync(ResendVerificationTokenDTO resendVerificationTokenDTO)
    {
        var token = await _verificationTokenService.CreateVerificationToken(resendVerificationTokenDTO.Email);

        var verificationUrl = $"{_configuration["ClientURL"]}api/auth/verify-email?token={token}";
        var emailBody = _emailTemplateService.GetRetryVerificationEmailBody(verificationUrl);

        await _emailService.SendEmailAsync(resendVerificationTokenDTO.Email, "Verify Your Email Address", emailBody);
    }
}