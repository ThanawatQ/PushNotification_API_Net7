﻿using DataAccess.Models.PushNotification;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Push_Service.Interface;
using Push_Service.Model;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebPush;

namespace Push_Service.Services
{
    public class PushNotificationQueue : IPushNotificationQueue
    {

        ConcurrentQueue<PushMessage> _messages = new ConcurrentQueue<PushMessage>();
        SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(0);

        private readonly IPushinitialize _pushinitial;
        private readonly IConfiguration _configuration;
        private readonly SubcriptionContext _context;
        private readonly IPushNotificationService _service;


        public PushNotificationQueue(SubcriptionContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _pushinitial = new Pushinitialize(_configuration);
            _context = context;
            _service = new PushNotificationService(_context);
        }
        public async Task SentNotification(SentNotificationVM message)
        {
            try
            {
                if (message == null)
                {
                    throw new ArgumentNullException();
                }

                List<Pushnotification> data = new List<Pushnotification>();
                if (message.SentAll == true)
                {
                    data = await _context.Pushnotifications
                     .Where(w => w.IsDelete != true)
                     .ToListAsync();
                }
                else if (message.Groups != null && message.Groups.Count > 0 )
                {
                    foreach (var group in message.Groups)
                    {
                        var newData = await _context.Pushnotifications
                        .Where(w => w.IsDelete != true && w.Group == group)
                        .ToListAsync();

                        data.AddRange(newData);
                    }
                }
                else
                {
                    foreach (var userId in message.UserId)
                    {
                        var newData = await _context.Pushnotifications
                        .Where(w => w.IsDelete != true && w.UserId == userId)
                        .ToListAsync();

                        data.AddRange(newData);
                    }
                }


                var vapidDetails = await _pushinitial.GetPushSubscript();
                await SendNotificationsAsync(data, message, vapidDetails);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task SendNotificationsAsync(IEnumerable<Pushnotification> data, SentNotificationVM messageContent, VapidDetails vapidDetails)
        {
            var tasks = data.Select(async item =>
            {
                try
                {
                    string logId = Guid.NewGuid().ToString();

                    messageContent.Content.UserId = logId;
                    var dataMessage = JsonConvert.SerializeObject(messageContent.Content);

                    var pushSubscription = new WebPush.PushSubscription(item.EndPoint, item.P256dh, item.Auth);
                    var webPush = new WebPushClient();
                    await webPush.SendNotificationAsync(pushSubscription, dataMessage, vapidDetails);

                    await _service.SentLogger(item, dataMessage);

                }
                catch (Exception ex)
                {
                    WebPushException webPushException = ex as WebPushException;

                    if ((webPushException.StatusCode == HttpStatusCode.NotFound) || (webPushException.StatusCode == HttpStatusCode.Gone))
                    {
                        await _service.DiscardSubscription(item.EndPoint);
                    }

                }

            });
            await Task.WhenAll(tasks);
        }


    }
}
