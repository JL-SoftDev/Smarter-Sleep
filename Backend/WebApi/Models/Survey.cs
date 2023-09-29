using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class Survey
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? SleepQuality { get; set; }

    public int? SleepDuration { get; set; }

    public DateOnly SurveyDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<SleepReview> SleepReviews { get; set; } = new List<SleepReview>();
}
