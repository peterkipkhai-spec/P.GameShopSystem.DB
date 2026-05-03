using System;
using System.Collections.Generic;

namespace P.GameShopSystem.DB.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int GameId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
