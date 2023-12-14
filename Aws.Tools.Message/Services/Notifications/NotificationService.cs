using Amazon.SimpleEmail.Model;
using Aws.Tools.Message.Services.Notifications.SES;
using Aws.Tools.Message.Services.Notifications.SES.Templates;
using Aws.Tools.Message.Services.Notifications.SNS;
using Aws.Tools.Message.Services.Notifications.WhatsApp;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
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

        public Task CreateTemplateAsync(string templateName, string subjectPart, string htmlPart)
        {
            return _sesClient.CreateTemplateAsync(templateName, subjectPart, htmlPart);
        }

        public async Task DeleteTemplateAsync(string templateName)
        {
            await _sesClient.DeleteTemplateAsync(templateName);
        }

        public async Task<GetTemplateResponse> GetTemplateAsync(string templateName)
        {
            return await _sesClient.GetTemplateAsync(templateName);
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

        public async Task SendWhatsAppMessage(WhatsAppTemplateMessageModel message)
        {
            await _whatAppService.SendWhatsAppMessage(message);
        }

        public async Task UpdateTemplateAsync(string templateName, string subjectPart, string htmlPart)
        {
            await _sesClient.UpdateTemplateAsync(templateName, subjectPart, htmlPart);
        }
    }
}
