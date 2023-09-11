using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class WearableData
{
    public int Id { get; set; }

    public DateTime? SleepStart { get; set; }

    public DateTime? SleepEnd { get; set; }

    public string? Hynogram { get; set; }

    public int? SleepScore { get; set; }

    public DateOnly SleepDate { get; set; }

    public virtual ICollection<SleepReview> SleepReviews { get; set; } = new List<SleepReview>();
}
