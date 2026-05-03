namespace P.GameShopSystem.API.Contracts.Games;

public sealed record GameDto(
    int GameId,
    string Title,
    int CategoryId,
    decimal Price,
    int StockQuantity,
    string? GameCondition,
    bool IsActive);

public sealed record UpsertGameRequestDto(
    string Title,
    int CategoryId,
    decimal Price,
    int StockQuantity,
    string? GameCondition,
    bool IsActive);
