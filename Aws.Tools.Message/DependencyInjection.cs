using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Aws.Tools.Message.Services.Messages.SES;
using Aws.Tools.Message.Services.Messages.SNS;
using Aws.Tools.Message.Services.Notifications;
using Aws.Tools.Message.Services.Notifications.Processors;
using Aws.Tools.Message.Services.Notifications.SES.Configuration;
using Aws.Tools.Message.Services.Notifications.WhatsApp.Configuration;
using Aws.Tools.Message.Services.Storage.S3;
using Aws.Tools.Message.Services.Storage.S3.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aws.Tools.Message
{
    public static class DependencyInjection
    {
        public static void AddAWSToolsLib(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<MessageConfiguration>(configuration.GetSection("MessageConfigurationPath"));
            _ = services.Configure<S3BucketConfiguration>(configuration.GetSection("S3BucketConfig"));

            _ = services.AddTransient(typeof(IMessageProcessor<>), typeof(MessageProcessor<>));
            _ = services.AddTransient<INotificationService, NotificationService>();

            _ = services.AddScoped<IS3Bucket, S3Bucket>();
            _ = services.AddAWSService<IAmazonSQS>();
            _ = services.AddAWSService<IAmazonSimpleNotificationService>();
            _ = services.AddTransient<ISNSClient, SNSClient>();
            _ = services.AddTransient<ISESClient, SESClient>();
            services.AddWhatsAppApi(configuration.GetSection("WhatsAppConfig"));
        }
    }
}
