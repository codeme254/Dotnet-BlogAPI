using BlogAPI.DTOs;
using BlogAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService, IValidator<RegisterDTO> validator) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IValidator<RegisterDTO> _validator = validator;

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var validationResults = await _validator.ValidateAsync(registerDTO);

        if (!validationResults.IsValid)
        {
            var errors = validationResults.Errors.Select(e => e.ErrorMessage);

            return BadRequest(new
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "User registration failed",
                Errors = errors
            });
        }
        await _authService.RegisterAsync(registerDTO);

        return CreatedAtAction(null, new
        {
            Message = "User created successfully"
        });
    }
}