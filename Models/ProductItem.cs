using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class ProductItem
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? ColorImage { get; set; }

    public int? QtyInStock { get; set; }

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductItemImage> ProductItemImages { get; set; } = new List<ProductItemImage>();

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
}
