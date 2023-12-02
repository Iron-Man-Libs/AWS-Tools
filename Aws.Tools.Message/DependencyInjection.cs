using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Aws.Tools.Message.Services.Messages.Configuration;
using Aws.Tools.Message.Services.Messages.Processors;
using Aws.Tools.Message.Services.Notifications;
using Aws.Tools.Message.Services.Storage.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Aws.Tools.Message
{
    public static class DependencyInjection
    {
        public static void AddAWSToolsLib(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<MessageConfiguration>(configuration.GetSection("MessageConfigurationPath") ?? throw new ArgumentNullException("MessageConfigurationPath is missing"));
            _ = services.Configure<S3BucketConfiguration>(configuration.GetSection("S3BucketConfig") ?? throw new ArgumentNullException("S3BucketConfig is missing"));

            _ = services.AddTransient(typeof(IMessageProcessor<>), typeof(MessageProcessor<>));
            _ = services.AddTransient<INotificationService, NotificationService>();

            _ = services.AddScoped<IS3Bucket, S3Bucket>();
            _ = services.AddAWSService<IAmazonSQS>();
            _ = services.AddAWSService<IAmazonSimpleNotificationService>();
        }
    }
}
