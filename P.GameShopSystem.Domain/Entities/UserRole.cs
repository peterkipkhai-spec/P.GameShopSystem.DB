namespace P.GameShopSystem.Domain.Entities;

public static class UserRole
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static readonly HashSet<string> All =
    [
        Admin,
        Customer
    ];
}
