using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class SiteUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = new List<UserPaymentMethod>();

    public virtual ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
}
