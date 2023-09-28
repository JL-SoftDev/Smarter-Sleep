using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class PurchaseLog
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int TransactionId { get; set; }

    [JsonIgnore]
    public virtual Item Item { get; set; } = null!;

    [JsonIgnore]
    public virtual Transaction Transaction { get; set; } = null!;
}
