using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Aws.Tools.Message.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SES
{
    public class SESClient : ISESClient
    {
        public async Task SendEmail(SESMessage message)
        {
            using AmazonSimpleEmailServiceClient client = new(RegionEndpoint.USEast1);
            SendEmailRequest request = new()
            {
                Source = message.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = message.ReceiversAddress
                },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Content(message.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message.Body
                        }
                    }
                }
            };

            _ = await client.SendEmailAsync(request);
        }

        public async Task SendEmailWithTemplate<T>(SESTemplateMessage<T> templateMessage)
        {
            using AmazonSimpleEmailServiceClient client = new(RegionEndpoint.USEast1);
            SendTemplatedEmailRequest request = new()
            {
                Source = templateMessage.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = templateMessage.ReceiversAddress
                },
                Template = templateMessage.TemplateName,
                TemplateData = JsonSerializer.Serialize(templateMessage.TemplateModel, new JsonSerializerOptions().Default())
            };

            _ = await client.SendTemplatedEmailAsync(request);
        }
    }
}
