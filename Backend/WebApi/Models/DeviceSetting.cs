using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class DeviceSetting
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public int? SleepSettingId { get; set; }

    public DateTime ScheduledTime { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Settings { get; set; }

    [JsonIgnore]
    public virtual SleepSetting? SleepSetting { get; set; }
}