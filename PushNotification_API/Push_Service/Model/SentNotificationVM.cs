using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push_Service.Model
{
    public class SentNotificationVM
    {
        public content Content { get; set; }
        public List<string>? UserId { get; set; } = null;
        public List<string>? Groups { get; set; } = null;
        public bool SentAll { get; set; }   = false;

    }
    public class content
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public List<string> Url { get; set; }
        public List<Actions> actions { get; set; }

    }
    public class Actions
    {
        public string action { get;set; }
        public string type { get; set; } = "button";
        public string title { get; set; }
    }
}
