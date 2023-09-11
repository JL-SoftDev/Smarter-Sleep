using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime? Timestamp { get; set; }

    public int PointAmount { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ChallengeLog> ChallengeLogs { get; set; } = new List<ChallengeLog>();

    public virtual ICollection<PurchaseLog> PurchaseLogs { get; set; } = new List<PurchaseLog>();

    public virtual AppUser User { get; set; } = null!;
}
