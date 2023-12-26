using System;
using System.Collections.Generic;

namespace DataAccess.Models.PushNotification;

public partial class Log
{
    public Guid LogId { get; set; }

    public Guid? SubId { get; set; }

    public DateTime? SentDate { get; set; }

    public string? SentDetail { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public string? ReceivedDetail { get; set; }

    public string? Status { get; set; }

    public virtual Pushnotification? Sub { get; set; }
}
