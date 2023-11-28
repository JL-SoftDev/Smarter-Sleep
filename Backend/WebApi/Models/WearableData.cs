using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class WearableData
{
    public int Id { get; set; }

    public DateTime? SleepStart { get; set; }

    public DateTime? SleepEnd { get; set; }

    public string? Hypnogram { get; set; }

    public int? SleepScore { get; set; }

    public DateOnly SleepDate { get; set; }

    [JsonIgnore]
    public virtual SleepReview SleepReview { get; set; }
}
