namespace P.GameShopSystem.API.Contracts.Categories;

public sealed record CategoryDto(int CategoryId, string Name, string? Description);

public sealed record UpsertCategoryRequestDto(string Name, string? Description);
