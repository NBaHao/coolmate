using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

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

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
}
