using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class ProductItemImage
{
    public int Id { get; set; }

    public int? ProductItemId { get; set; }

    public string? Url { get; set; }

    public virtual ProductItem? ProductItem { get; set; }
}
