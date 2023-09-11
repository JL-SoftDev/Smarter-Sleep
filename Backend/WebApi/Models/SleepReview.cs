using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class SleepReview
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public int? WearableLogId { get; set; }

    public int? SurveyId { get; set; }

    public int? SmarterSleepScore { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual AppUser User { get; set; } = null!;

    public virtual WearableData? WearableLog { get; set; }
}
