using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? PriceInt { get; set; }

    public string? PriceStr { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
