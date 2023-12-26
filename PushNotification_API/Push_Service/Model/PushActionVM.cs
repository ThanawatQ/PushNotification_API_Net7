using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push_Service.Model
{
    public class PushActionVM
    {
        public string Url { get;set; }
        public string UserId { get; set; }
        public string Response { get;set; }
        public string ActionType { get;set; }
        public string Status { get; set; } = "Received";
    }
}
