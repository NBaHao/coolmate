using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class ShoppingCartItem
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public int? ProductItemId { get; set; }

    public int? Qty { get; set; }

    public virtual ShoppingCart? Cart { get; set; }

    public virtual ProductItem? ProductItem { get; set; }
}
