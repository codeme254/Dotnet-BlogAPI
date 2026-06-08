using BlogAPI.DTOs;

namespace BlogAPI.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDTO registerDTO);
}