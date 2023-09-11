using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class DailyStreak
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly LastDate { get; set; }

    public virtual AppUser User { get; set; } = null!;
}
