using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class AppUser
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? Points { get; set; }

    [JsonIgnore]
    public virtual DailyStreak? DailyStreak { get; set; }

    [JsonIgnore]
    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    [JsonIgnore]
    public virtual ICollection<SleepReview> SleepReviews { get; set; } = new List<SleepReview>();

    [JsonIgnore]
    public virtual SleepSetting? SleepSetting { get; set; }

    [JsonIgnore]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [JsonIgnore]
    public virtual ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
}
