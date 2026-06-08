using BlogAPI.Data;
using BlogAPI.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Validators;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    private readonly AppDbContext _dbContext;
    public RegisterDTOValidator(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(u => u.Username)
        .NotEmpty().WithMessage("Username is required")
        .MustAsync(BeUniqueUsernameAsync).WithMessage("Username is already taken");

        RuleFor(u => u.Email)
        .NotEmpty().WithMessage("Email address is required")
        .EmailAddress().WithMessage("Invalid email format")
        .MustAsync(BeUniqueEmailAsync).WithMessage("Email address is already taken");

        RuleFor(u => u.Password)
        .NotEmpty().WithMessage("Password is required")
        .MinimumLength(8).WithMessage("Password should be at least 8 characters long")
        .Must(BeComplexPassword)
        .WithMessage("Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character");
        // .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
        // .WithMessage("Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character");
    }

    // Return true if username is not taken
    private async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        return user == null;
    }

    // Return true if email address is not taken
    private async Task<bool> BeUniqueEmailAsync(string emailAddress, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == emailAddress, cancellationToken);

        return user == null;
    }

    // Return true if password is complex
    private static bool BeComplexPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;

        return password.Any(char.IsUpper)
        && password.Any(char.IsLower)
        && password.Any(char.IsDigit)
        && password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}