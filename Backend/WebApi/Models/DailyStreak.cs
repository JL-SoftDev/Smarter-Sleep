using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class DailyStreak
{
    public Guid UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly LastDate { get; set; }
}
