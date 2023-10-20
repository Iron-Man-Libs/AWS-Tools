using System.Collections.Generic;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public class SESMessage
    {
        public string SenderAddress { get; set; }
        public List<string> ReceiversAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
