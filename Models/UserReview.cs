using System;
using System.Collections.Generic;

namespace WebApplication1.Entities;

public partial class UserReview
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? OrderedProductId { get; set; }

    public int? RatingValue { get; set; }

    public string? Comment { get; set; }

    public virtual OrderLine? OrderedProduct { get; set; }

    public virtual SiteUser? User { get; set; }
}
