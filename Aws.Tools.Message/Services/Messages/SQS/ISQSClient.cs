using System;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SQS
{
    public interface ISQSClient
    {
        Task GetAllMessagesAsync<T>(string queueName, Func<T, Task> handle);

        Task<bool> PublishMessageAsync<T>(string queueName, T messageBody);
    }
}
