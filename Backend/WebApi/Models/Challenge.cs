using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Challenge
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Reward { get; set; }

    public virtual ICollection<ChallengeLog> ChallengeLogs { get; set; } = new List<ChallengeLog>();
}
