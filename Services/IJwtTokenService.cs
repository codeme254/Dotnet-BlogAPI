using BlogAPI.Models;

namespace BlogAPI.Services;

public interface IJwtTokenService
{
    string GenerateJwtToken(User user);
}