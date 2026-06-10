using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Repositories.Implementations;

public class VerificationTokenRepository(AppDbContext dbContext) : IVerificationTokenRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public void AddToken(VerificationToken verificationToken)
    {
        _dbContext.VerificationTokens.Add(verificationToken);
    }

    public void DeleteVerificationToken(VerificationToken verificationToken)
    {
        _dbContext.VerificationTokens.Remove(verificationToken);
    }

    public async Task<VerificationToken?> GetVerificationTokenAsync(string identifier)
    {
        return await _dbContext.VerificationTokens
        .FirstOrDefaultAsync(t => t.Email == identifier || t.Token == identifier);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}