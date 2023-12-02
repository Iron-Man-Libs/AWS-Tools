using System.Collections.Generic;

namespace Aws.Tools.Message.Services.Notifications.SES.Templates
{
    public class SESMessage
    {
        public string SenderAddress { get; set; }
        public List<string> ReceiversAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
