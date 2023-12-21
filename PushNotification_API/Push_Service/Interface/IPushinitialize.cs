using DataAccess.Models.PushNotification;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPush;

namespace Push_Service.Interface
{
    public interface IPushinitialize
    {
        Task<string> GetPublicKey();
        Task<VapidDetails> GetPushSubscript();

    }
}
