using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Device
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Ip { get; set; }

    public int? Port { get; set; }

    public string? Status { get; set; }

    public virtual AppUser User { get; set; } = null!;
}
