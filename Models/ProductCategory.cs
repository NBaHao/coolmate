using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public int? ParentCategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
