using DataAccess.Models.PushNotification;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public PushNotificationService(SubcriptionContext context)
        {
            _context = context;
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
                        Group = subscription.Group,
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


        public async Task DiscardSubscription(string EndPoint)
        {
            try
            {
                var data = await _context.Pushnotifications.FirstOrDefaultAsync(e => e.EndPoint == EndPoint && e.IsDelete != true);
                data.IsDelete = true;

                _context.Pushnotifications.Update(data);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task SentLogger(Pushnotification data, string datalog)
        {
            try
            {
                var dataobj = JsonConvert.DeserializeObject<content>(datalog);
                var logData = new Log
                {
                    LogId = Guid.Parse(dataobj.UserId),
                    SubId = data.SubId,
                    SentDate = DateTime.Now,
                    SentDetail = datalog,
                    Status = "Sent",
                };
                await _context.Logs.AddAsync(logData);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateLogger(PushActionVM obj)
        {
            try
            {
                var data = await _context.Logs
                    .Include(e => e.Sub).Where(w => w.Sub.IsDelete != true)
                    .Where(w => w.Status == "Sent" || w.Status == "Received" && w.LogId.ToString() == obj.UserId)
                    .ToListAsync();

                if (data.Count > 0 && data != null)
                {
                    var tasks = data.Select(async item =>
                    {
                        item.ReceivedDate = DateTime.Now;
                        item.Status = obj.Status;
                        item.ReceivedDetail = JsonConvert.SerializeObject(obj);
                        _context.Logs.Update(item);
                        await _context.SaveChangesAsync();
                    });

                    await Task.WhenAll(tasks);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
