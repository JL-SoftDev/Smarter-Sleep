using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class AppUser
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? Points { get; set; }

    public virtual DailyStreak? DailyStreak { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual ICollection<SleepReview> SleepReviews { get; set; } = new List<SleepReview>();

    public virtual SleepSetting? SleepSetting { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
