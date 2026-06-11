using BlogAPI.DTOs;
using BlogAPI.Models;

namespace BlogAPI.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDTO registerDTO);
    Task VerifyEmailAsync(string token);
    Task ResendVerificationTokenAsync(ResendVerificationTokenDTO resendVerificationTokenDTO);
    Task<string> LoginAsync(LoginDTO loginDTO);
}