using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Cost { get; set; }

    public virtual ICollection<PurchaseLog> PurchaseLogs { get; set; } = new List<PurchaseLog>();
}
