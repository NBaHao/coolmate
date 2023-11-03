using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class UserAddress
{
    public int? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? IsDefault { get; set; }

    public virtual Address? Address { get; set; }

    public virtual SiteUser? User { get; set; }
}
