using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolMate.Models;

public partial class SiteUser : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string Id { get; set; }
    public string? Name { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? Height { get; set; }
    public int? Weight { get; set; }
    public string? Birthday { get; set; }

    public virtual ICollection<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
}
