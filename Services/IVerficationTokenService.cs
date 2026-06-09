namespace BlogAPI.Services;

public interface IVerificationTokenService
{
    Task<string> CreateVerificationToken(string email);
}