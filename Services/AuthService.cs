
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelpDeskSystem.DTOs.Auth;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HelpDeskSystem.Services;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("LOGIN METHOD WAS CALLED!");

        // Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var isLockedOut = await _userManager.IsLockedOutAsync(user);

        Console.WriteLine($"User {user.Email} is locked out: {isLockedOut}");

        // Verify password
        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true
        );

        if (!signInResult.Succeeded)
        {
            return null;
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Create JWT claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName ?? user.Email!)
        };

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        // JWT configuration
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;

        var expirationMinutes = _configuration.GetValue<int>(
            "Jwt:ExpirationInMinutes");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            expirationMinutes);

        // Generate JWT
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        // Return authentication response
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAt,
            User = new CurrentUserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles
            }
        };
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        var userId = _userManager.GetUserId(principal);

        if (userId is null)
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new CurrentUserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = $"{user.FirstName} {user.LastName}",
            Roles = roles
        };
    }

    public async Task LogoutAsync(
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        // Token revocation will be implemented
        // when refresh-token management is added.
        await Task.CompletedTask;
    }
}
