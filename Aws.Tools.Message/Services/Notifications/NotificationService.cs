using Aws.Tools.Message.Services.Messages.SES;
using Aws.Tools.Message.Services.Messages.SNS;
using Aws.Tools.Message.Services.Notifications.SES.Templates;
using Aws.Tools.Message.Services.Notifications.WhatsApp;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ISNSClient _snsClient;
        private readonly ISESClient _sesClient;
        private readonly IWhatsAppService _whatAppService;

        public NotificationService(ISNSClient snsClient, ISESClient sesClient, IWhatsAppService whatAppService)
        {
            _snsClient = snsClient;
            _sesClient = sesClient;
            _whatAppService = whatAppService;
        }

        public async Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null)
        {
            await _snsClient.PublishMessageAsync(topicName, message, entityName, apiName);
        }

        public async Task PublishSMSMessageAsync(string phoneNumber, string message)
        {
            await _snsClient.PublishSMSMessageAsync(phoneNumber, message);
        }

        public async Task SendEmailAsync(SESMessage emailMessage)
        {
            await _sesClient.SendEmailAsync(emailMessage);
        }

        public async Task SendEmailAsync<T>(SESTemplateMessage<T> templateMessage)
        {
            await _sesClient.SendEmailAsync(templateMessage);
        }

        public async Task SendTemplateMessage(TemplateMessageModel message)
        {
            await _whatAppService.SendTemplateMessage(message);
        }

        public async Task SendTextMessage(TextMessageModel message)
        {
            await _whatAppService.SendTextMessage(message);
        }
    }
}
