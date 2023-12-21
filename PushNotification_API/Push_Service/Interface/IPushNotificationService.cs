using Push_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push_Service.Interface
{
    public interface IPushNotificationService
    {
        Task Subsctiption(PushSubscriptVM subscription);
        Task DiscardSubscriptionAsync(string endpoint);
    }
}
