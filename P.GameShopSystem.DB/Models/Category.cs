using System;
using System.Collections.Generic;

namespace P.GameShopSystem.DB.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
