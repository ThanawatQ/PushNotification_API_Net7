﻿using System;
using System.Collections.Generic;

namespace DataAccess.Models.PushNotification;

public partial class Pushnotification
{
    public Guid SubId { get; set; }

    public string? EndPoint { get; set; }

    public string? P256dh { get; set; }

    public string? Auth { get; set; }

    public string? UserId { get; set; }

    public string? Group { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
