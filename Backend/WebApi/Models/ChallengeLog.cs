using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class ChallengeLog
{
    public int Id { get; set; }

    public int ChallengeId { get; set; }

    public int TransactionId { get; set; }

    public virtual Challenge Challenge { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
