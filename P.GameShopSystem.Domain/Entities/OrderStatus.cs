namespace P.GameShopSystem.Domain.Entities;

public static class OrderStatus
{
    public const string Pending = "Pending";
    public const string Processing = "Processing";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    public static readonly HashSet<string> All =
    [
        Pending,
        Processing,
        Completed,
        Cancelled
    ];
}
