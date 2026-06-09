using BlogAPI.Models;

namespace BlogAPI.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetUserAsync(string email);
    Task SaveChangesAsync();
}