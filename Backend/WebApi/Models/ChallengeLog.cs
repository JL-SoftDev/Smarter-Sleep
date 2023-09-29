using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class ChallengeLog
{
    public int Id { get; set; }

    public int ChallengeId { get; set; }

    [JsonIgnore]
    public int TransactionId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
