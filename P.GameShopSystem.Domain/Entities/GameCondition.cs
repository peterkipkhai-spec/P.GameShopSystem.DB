namespace P.GameShopSystem.Domain.Entities;

public static class GameCondition
{
    public const string New = "New";
    public const string LikeNew = "LikeNew";
    public const string Used = "Used";

    public static readonly HashSet<string> All =
    [
        New,
        LikeNew,
        Used
    ];
}
