using WebPush;
using Lib.AspNetCore.WebPush;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Push_Service.Interface;
using DataAccess.Models.PushNotification;

namespace Push_Service.Services
{
    public class Pushinitialize : IPushinitialize
    {
        private readonly IConfiguration _configuration;
        public Pushinitialize(IConfiguration  configuration) 
        { 
            _configuration = configuration;
        }

        public async Task<string> GetPublicKey()
        {
            IConfigurationSection pushNotificationServiceConfigurationSection = _configuration.GetSection(nameof(PushServiceClient));
            return pushNotificationServiceConfigurationSection.GetValue<string>("PublicKey"); 
        }
        public async  Task<VapidDetails> GetPushSubscript()
        {
         
            IConfigurationSection pushNotificationServiceConfigurationSection = _configuration.GetSection(nameof(PushServiceClient));
            var Subject =   pushNotificationServiceConfigurationSection.GetValue<string>("Subject");
            var PublicKey = pushNotificationServiceConfigurationSection.GetValue<string>("PublicKey");
            var PrivateKey = pushNotificationServiceConfigurationSection.GetValue<string>("PrivateKey");
            var subsciption = new VapidDetails(Subject, PublicKey, PrivateKey);

            return subsciption;
        }


    }
}
