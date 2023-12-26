using DataAccess.Models.PushNotification;
using Lib.Net.Http.WebPush;
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
        Task DiscardSubscription(string EndPoint);
        Task SentLogger(Pushnotification data,string datalog);
        Task UpdateLogger(PushActionVM userId);

    }
}
