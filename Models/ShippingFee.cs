using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class ShippingFee
{
    public int Id { get; set; }

    public int? ShippingAddress { get; set; }

    public int? Value { get; set; }

    public virtual Address? ShippingAddressNavigation { get; set; }
}
