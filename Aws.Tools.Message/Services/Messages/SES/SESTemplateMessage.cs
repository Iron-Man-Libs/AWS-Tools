using System.Collections.Generic;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public class SESTemplateMessage
    {
        public string SenderAddress { get; set; }
        public List<string> ReceiversAddress { get; set; }
        public string TemplateName { get; set; }
        public string TemplateModel { get; set; }
    }
}
