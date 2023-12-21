using Lib.Net.Http.WebPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push_Service.Model
{
    public class PushSubscriptVM
    {
        public PushSubscription subscription { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }  
    }
}
