﻿using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class UserPaymentMethod
{
    public int Id { get; set; }

    public String? UserId { get; set; }

    public int? PaymentTypeId { get; set; }

    public string? Provider { get; set; }

    public string? AccountNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int? IsDefault { get; set; }

    public virtual PaymentType? PaymentType { get; set; }

    public virtual ICollection<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();

    public virtual SiteUser? User { get; set; }
}
