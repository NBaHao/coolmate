using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class ShopOrder
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? PaymentMethodId { get; set; }

    public int? ShippingAddress { get; set; }

    public int? ShippingMethod { get; set; }

    public int? OrderTotal { get; set; }

    public int? OrderStatus { get; set; }

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    public virtual OrderStatus? OrderStatusNavigation { get; set; }

    public virtual UserPaymentMethod? PaymentMethod { get; set; }

    public virtual Address? ShippingAddressNavigation { get; set; }

    public virtual ShippingMethod? ShippingMethodNavigation { get; set; }

    public virtual SiteUser? User { get; set; }
}
