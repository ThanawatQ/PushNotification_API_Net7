using DataAccess.Models.PushNotification;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Push_Service.Interface;
using Push_Service.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Push_Service.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly SubcriptionContext _context;
        private readonly IPushNotificationQueue _queue;
        public PushNotificationService(SubcriptionContext context, IPushNotificationQueue queue)
        {
            _context = context;
            _queue = queue;
        }

        public async Task Subsctiption(PushSubscriptVM subscription)
        {
            try
            {
                string P256dh = subscription.subscription.Keys.FirstOrDefault(kvp => kvp.Key == "p256dh").Value ?? "";
                string Auth = subscription.subscription.Keys.FirstOrDefault(kvp => kvp.Key == "auth").Value ?? "";

                var check = await _context.Pushnotifications.Where(w => w.P256dh == P256dh && w.Auth == Auth && w.UserId == subscription.UserId).AsNoTracking().AnyAsync();
                if (check)
                {
                    throw new ArgumentOutOfRangeException(nameof(check), "Data Duplicate!.");
                }
                else
                {
                    Pushnotification obj = new Pushnotification
                    {
                        Role = subscription.Role,
                        UserId = subscription.UserId,
                        EndPoint = subscription.subscription.Endpoint,
                        P256dh = P256dh,
                        Auth = Auth,
                        IsDelete = false,
                    };
                    var data = await _context.Pushnotifications.AddAsync(obj);
                }
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DiscardSubscriptionAsync(string endpoint)
        {
            var data = await _context.Pushnotifications.FindAsync(endpoint);
            _context.Pushnotifications.Remove(data);

            await _context.SaveChangesAsync();
        }
    }
}
