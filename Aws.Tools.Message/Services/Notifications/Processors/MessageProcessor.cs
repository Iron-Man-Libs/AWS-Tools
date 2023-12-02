using System;
using System.Text.Json;
using System.Threading.Tasks;
using Aws.Tools.Message.Serialization;
using Aws.Tools.Message.Services.Notifications.Processors;
using Microsoft.Extensions.Logging;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Aws.Tools.Message.Services.Messages.Processors
{
    public class MessageProcessor<T> : IMessageProcessor<T> where T : class
    {
        private readonly IMessageHandler<T> _handler;
        private readonly ILogger<MessageProcessor<T>> _logger;

        public MessageProcessor(IMessageHandler<T> handler, ILogger<MessageProcessor<T>> logger)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Process(SQSMessage message)
        {
            try
            {
                var messageEntity = JsonSerializer.Deserialize<T>(message.Body, new JsonSerializerOptions().Default());
                await _handler.Process(messageEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_PROCESS_MESSAGE {body}", message.Body);
            }
        }
    }
}

