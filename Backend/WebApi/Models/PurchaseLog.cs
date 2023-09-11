using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class PurchaseLog
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int TransactionId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
