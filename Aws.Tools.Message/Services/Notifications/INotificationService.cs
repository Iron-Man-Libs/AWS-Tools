﻿using Aws.Tools.Message.Services.Messages.SES;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications
{
    public interface INotificationService
    {
        Task SendEmailAsync(SESMessage message);
        Task SendEmailAsync<T>(SESTemplateMessage<T> template);
        Task PublishMessageAsync<T>(string topicName, T message, string entityName = null, string apiName = null);
        Task PublishSMSMessageAsync(string number, string message);
    }
}