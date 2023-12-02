using Aws.Tools.Message.Services.Notifications.SES.Templates;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public interface ISESClient
    {
        Task SendEmailAsync(SESMessage message);
        Task SendEmailAsync<T>(SESTemplateMessage<T> templateMessage);
    }
}
