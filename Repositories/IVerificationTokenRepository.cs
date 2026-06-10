using BlogAPI.Models;

namespace BlogAPI.Repositories;

public interface IVerificationTokenRepository
{
    void AddToken(VerificationToken verificationToken);
    Task<VerificationToken?> GetVerificationTokenAsync(string identifier);
    void DeleteVerificationToken(VerificationToken verificationToken);
    Task SaveChangesAsync();
}