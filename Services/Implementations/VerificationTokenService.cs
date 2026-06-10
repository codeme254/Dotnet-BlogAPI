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
        // Check if this email already has a token
        // if it already has a verification token, delete it and create a new one
        var existing = await _tokenRepository.GetVerificationTokenAsync(email);
        if (existing != null)
        {
            _tokenRepository.DeleteVerificationToken(existing);
            await _tokenRepository.SaveChangesAsync();
        }

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