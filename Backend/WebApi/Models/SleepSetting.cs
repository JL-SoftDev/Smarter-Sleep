using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class SleepSetting
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime ScheduledSleep { get; set; }

    public DateTime ScheduledWake { get; set; }

    public string? ScheduledHypnogram { get; set; }

    [JsonIgnore]
    public virtual AppUser User { get; set; } = null!;
}
