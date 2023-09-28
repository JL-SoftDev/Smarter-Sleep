using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

public partial class CustomSchedule
{
    public Guid UserId { get; set; }

    [Column("day_of_week")]
    public DayOfWeekEnum DayOfWeek { get; set; }

    public TimeOnly? WakeTime { get; set; }
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