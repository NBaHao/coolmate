using CoolMate.Models;
using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? ProductItemId { get; set; }

    public string? Url { get; set; }

    public virtual ProductItem? ProductItem { get; set; }
}
