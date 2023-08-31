using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aws.Tools.Message.SQS
{
    public interface ISQSClient
    {
        Task GetAllMessagesAsync<T>(string queueName, Func<T, Task> handle);

        Task<bool> PublishMessageAsync<T>(string queueName, T messageBody);
    }
}
