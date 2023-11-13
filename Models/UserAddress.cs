using System;
using System.Collections.Generic;

namespace CoolMate.Models;

public partial class UserAddress
{
    public int? Id { get; set; }
    public String? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? IsDefault { get; set; }

    public virtual Address? Address { get; set; }

    public virtual SiteUser? User { get; set; }
}
