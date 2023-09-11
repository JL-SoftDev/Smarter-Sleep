using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Survey
{
    public int Id { get; set; }

    public int? SleepQuality { get; set; }

    public int? SleepDuration { get; set; }

    public DateOnly SurveyDate { get; set; }

    public virtual ICollection<SleepReview> SleepReviews { get; set; } = new List<SleepReview>();
}
