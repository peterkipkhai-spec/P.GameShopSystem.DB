namespace P.GameShopSystem.Web.Models.Catalog;

public sealed class GameCardViewModel
{
    public int GameId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? GameCondition { get; set; }
    public bool IsActive { get; set; }
}

public sealed class HomeCatalogViewModel
{
    public List<GameCardViewModel> Games { get; set; } = [];
}
