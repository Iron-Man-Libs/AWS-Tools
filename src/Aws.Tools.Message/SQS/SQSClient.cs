using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Aws.Tools.Message.Serializationn;
using Microsoft.Extensions.Logging;

namespace Aws.Tools.Message.SQS
{
    public class SQSClient : ISQSClient
    {
        private readonly IAmazonSQS _amazonSQS;
        private readonly ILogger<SQSClient> _logger;

        public SQSClient(IAmazonSQS amazonSQS, ILogger<SQSClient> logger)
        {
            _amazonSQS = amazonSQS;
            _logger = logger;
        }

        public async Task GetAllMessagesAsync<T>(string queueUrl, Func<T, Task> handle)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 5
            };

            var messageResponse = await _amazonSQS.ReceiveMessageAsync(request);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("UNABLE_TO_GET_MESSAGES");
            }
            else
            {
                foreach (SQSMessage message in messageResponse.Messages)
                {
                    try
                    {
                        var messageEntity = JsonSerializer.Deserialize<T>(message.Body, new JsonSerializerOptions().Default());
                        await handle(messageEntity);

                        await DeleteMessagesAsync(queueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "UNABLE_TO_PROCESS_THE_MESSAGE", message.ToString());
                    }
                }
            }
        }

        private async Task<bool> DeleteMessagesAsync(string queueUrl, string receiptHandle)
        {
            try
            {
                var result = await _amazonSQS.DeleteMessageAsync(queueUrl, receiptHandle);
                return result.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DELETE_MESSAGE_ERROR");
                return false;
            }
        }

        public async Task<bool> PublishMessageAsync<T>(string queueUrl, T messageBody)
        {
            try
            {
                var sendRequest = new SendMessageRequest(queueUrl, JsonSerializer.Serialize<T>(messageBody, new JsonSerializerOptions().Default()));

                var sendResult = await _amazonSQS.SendMessageAsync(sendRequest);

                _logger.LogInformation("MESSAGE_HAS_BEEN_SENT");

                return sendResult.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_PUBLISH_MESSAGE");
                return false;
            }
        }
    }
}
