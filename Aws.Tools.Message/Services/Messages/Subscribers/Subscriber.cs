using Aws.Tools.Message.Services.Messages.SQS;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.Subscribers
{
    public class Subscriber<T> : ISubscriber<T> where T : class
    {
        private readonly ISQSClient _sqsClient;
        private readonly IMessageHandler<T> _handler;

        public Subscriber(ISQSClient sqsClient, IMessageHandler<T> handler)
        {
            _sqsClient = sqsClient;
            _handler = handler;
        }

        public async Task Subscribe(string queueName)
        {
            await _sqsClient.GetAllMessagesAsync<T>(queueName, _handler.Process);
        }
    }
}

