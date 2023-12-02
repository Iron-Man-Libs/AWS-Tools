using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TextMessages;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp
{
    public interface IWhatsAppService
    {
        Task SendTextMessage(TextMessageModel message);

        Task SendTemplateMessage(TemplateMessageModel message);
    }
}
