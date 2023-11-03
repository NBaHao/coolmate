using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();
}
