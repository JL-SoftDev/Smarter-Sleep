using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class SleepReview
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public int? WearableLogId { get; set; }

    [JsonIgnore]
    public int? SurveyId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? SmarterSleepScore { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual WearableData? WearableLog { get; set; }
}
