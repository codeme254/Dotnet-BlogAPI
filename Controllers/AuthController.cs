using BlogAPI.DTOs;
using BlogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        await _authService.RegisterAsync(registerDTO);

        return CreatedAtAction(null, new
        {
            Message = "User created successfully"
        });
    }
}