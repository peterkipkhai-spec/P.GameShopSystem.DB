namespace P.GameShopSystem.API.Contracts.Auth;

public sealed record RegisterRequestDto(
    string FullName,
    string PhoneNumber,
    string? Email,
    string Password,
    string? Address);

public sealed record LoginRequestDto(string PhoneNumber, string Password);

public sealed record AuthResponseDto(
    int UserId,
    string FullName,
    string PhoneNumber,
    string? Email,
    string Role,
    string Token);
