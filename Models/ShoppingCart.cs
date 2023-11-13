using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class ShoppingCart
{
    public int Id { get; set; }

    public String? UserId { get; set; }

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

    public virtual SiteUser? User { get; set; }
}
