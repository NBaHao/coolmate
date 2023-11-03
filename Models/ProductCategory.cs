using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class ProductCategory
{
    public int Id { get; set; }

    public int? ParentCategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
