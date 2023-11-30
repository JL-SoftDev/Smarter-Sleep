using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int PointAmount { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ChallengeLog? ChallengeLog { get; set; }

    [JsonIgnore]
    public virtual PurchaseLog? PurchaseLog { get; set; }
}
