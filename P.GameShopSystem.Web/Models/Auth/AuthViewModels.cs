using System.ComponentModel.DataAnnotations;

namespace P.GameShopSystem.Web.Models.Auth;

public sealed class RegisterViewModel
{
    [Required, MaxLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string PhoneNumber { get; set; } = string.Empty;
    [EmailAddress] public string? Email { get; set; }
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [MaxLength(500)] public string? Address { get; set; }
}

public sealed class LoginViewModel
{
    [Required, MaxLength(20)] public string PhoneNumber { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public sealed class AuthResponseViewModel
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
