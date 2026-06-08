using BlogAPI.Data;
using BlogAPI.DTOs;
using BlogAPI.Models;
using IdGen;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[Route("api/[controller]")]
public class AuthController(AppDbContext appDbContext, IdGenerator idGen) : ControllerBase
{
    private readonly AppDbContext _dbContext = appDbContext;
    private readonly IdGenerator _idGen = idGen;

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var user = new User
        {
            UserId = _idGen.CreateId(),
            Username = registerDTO.Username,
            Email = registerDTO.Email,
            PasswordHash = registerDTO.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(null, new
        {
            Message = "User created successfully"
        });
    }
}