using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.GameShopSystem.API.Contracts.Auth;
using P.GameShopSystem.DB.Models;

namespace P.GameShopSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(GameShopDbContext dbContext) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var phone = request.PhoneNumber.Trim();
        var isTaken = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.PhoneNumber == phone, cancellationToken);

        if (isTaken)
        {
            return Conflict("Phone number already exists.");
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            PhoneNumber = phone,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            PasswordHash = Hash(request.Password),
            Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
            Role = "Customer"
        };

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(ToResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var phone = request.PhoneNumber.Trim();
        var passwordHash = Hash(request.Password);

        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.PhoneNumber == phone && u.PasswordHash == passwordHash, cancellationToken);

        if (user is null)
        {
            return Unauthorized("Invalid phone number or password.");
        }

        return Ok(ToResponse(user));
    }

    private static AuthResponseDto ToResponse(User user)
    {
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.UserId}:{user.PhoneNumber}:{DateTime.UtcNow:O}"));

        return new AuthResponseDto(
            user.UserId,
            user.FullName,
            user.PhoneNumber,
            user.Email,
            user.Role ?? "Customer",
            token);
    }

    private static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
