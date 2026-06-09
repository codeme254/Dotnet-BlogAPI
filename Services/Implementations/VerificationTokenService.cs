using BlogAPI.Models;
using BlogAPI.Repositories;
using IdGen;

namespace BlogAPI.Services.Implementations;

public class VerificationTokenService(IdGenerator idGen, IVerificationTokenRepository verificationTokenRepository) : IVerificationTokenService
{
    private readonly IdGenerator _idGen = idGen;
    private readonly IVerificationTokenRepository _tokenRepository = verificationTokenRepository;

    public async Task<string> CreateVerificationToken(string email)
    {
        string token = Guid.NewGuid().ToString();
        var verificationToken = new VerificationToken
        {
            Id = _idGen.CreateId(),
            Email = email,
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + TimeSpan.FromMinutes(30)
        };
        _tokenRepository.AddToken(verificationToken);
        await _tokenRepository.SaveChangesAsync();
        return token;
    }
}