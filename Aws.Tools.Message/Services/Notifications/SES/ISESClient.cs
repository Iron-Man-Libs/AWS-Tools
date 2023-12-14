using Amazon.SimpleEmail.Model;
using Aws.Tools.Message.Services.Notifications.SES.Templates;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.SES
{
    public interface ISESClient
    {
        Task SendEmailAsync(SESMessage message);
        Task SendEmailAsync<T>(SESTemplateMessage<T> templateMessage);
        Task CreateTemplateAsync(string templateName, string subjectPart, string htmlPart);
        Task UpdateTemplateAsync(string templateName, string subjectPart, string htmlPart);
        Task DeleteTemplateAsync(string templateName);
        Task<GetTemplateResponse> GetTemplateAsync(string templateName);
    }
}
