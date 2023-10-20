using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public interface ISESClient
    {
        Task SendEmail(SESMessage message);
        Task SendEmailWithTemplate(SESTemplateMessage templateMessage);
    }
}
