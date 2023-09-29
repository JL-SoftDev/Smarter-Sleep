using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class PurchaseLog
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    [JsonIgnore]
    public int TransactionId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
