using Aws.Tools.Message.Services.Notifications.SES.Templates;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications
{
    public interface INotificationService
    {
        Task SendEmailAsync(SESMessage message);
        Task SendEmailAsync<T>(SESTemplateMessage<T> template);
        Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null);
        Task PublishSMSMessageAsync(string number, string message);
        Task SendTextMessage(TextMessageModel message);
        Task SendTemplateMessage(TemplateMessageModel message);
    }
}
