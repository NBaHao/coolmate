using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = new List<UserPaymentMethod>();
}
