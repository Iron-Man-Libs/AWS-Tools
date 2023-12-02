using Aws.Tools.Message.Services.Notifications.WhatsApp.Models.TemplateMessages;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp
{
    public interface IWhatsAppService
    {
        Task SendWhatsAppMessage(WhatsAppTemplateMessageModel message);
    }
}
