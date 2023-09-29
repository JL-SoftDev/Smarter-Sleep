using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApi.Models;

public partial class UserChallenge
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public int ChallengeId { get; set; }

    public bool? Completed { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public bool UserSelected { get; set; }
}
