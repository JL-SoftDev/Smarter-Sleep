using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class CustomSchedule
{
    public Guid UserId { get; set; }

    [EnumDataType(typeof(DayOfWeekEnum))]
    public DayOfWeekEnum DayOfWeek { get; set; }

    public TimeOnly? WakeTime { get; set; }

    [JsonIgnore]
    public virtual AppUser User { get; set; } = null!;
}
public enum DayOfWeekEnum
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }