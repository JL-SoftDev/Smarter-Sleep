using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class Challenge
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Reward { get; set; }

    [JsonIgnore]
    public virtual ICollection<ChallengeLog> ChallengeLogs { get; set; } = new List<ChallengeLog>();

    [JsonIgnore]
    public virtual ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
}
