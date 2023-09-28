using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class ChallengeLog
{
    public int Id { get; set; }

    public int ChallengeId { get; set; }

    public int TransactionId { get; set; }

    [JsonIgnore]
    public virtual Challenge Challenge { get; set; } = null!;

    [JsonIgnore]
    public virtual Transaction Transaction { get; set; } = null!;
}
