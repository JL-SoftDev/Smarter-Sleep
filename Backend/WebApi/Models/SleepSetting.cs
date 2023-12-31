﻿using System;
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

    public virtual ICollection<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();

    [JsonIgnore]
    public virtual SleepReview SleepReview { get; set; }
}
