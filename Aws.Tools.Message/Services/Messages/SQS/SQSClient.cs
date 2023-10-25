using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Amazon.SQS.Model;
using Aws.Tools.Message.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SQS
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
            ReceiveMessageRequest request = new()
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 5
            };

            ReceiveMessageResponse messageResponse = await _amazonSQS.ReceiveMessageAsync(request);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("UNABLE_TO_GET_MESSAGES");
            }
            else
            {
                foreach (Amazon.SQS.Model.Message message in messageResponse.Messages)
                {
                    try
                    {
                        T messageEntity = JsonSerializer.Deserialize<T>(message.Body, new JsonSerializerOptions().Default());
                        await handle(messageEntity);

                        _ = await DeleteMessagesAsync(queueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "UNABLE_TO_PROCESS_THE_MESSAGE", message.MessageId);
                    }
                }
            }
        }

        private async Task<bool> DeleteMessagesAsync(string queueUrl, string receiptHandle)
        {
            try
            {
                DeleteMessageResponse result = await _amazonSQS.DeleteMessageAsync(queueUrl, receiptHandle);
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
                SendMessageRequest sendRequest = new(queueUrl, JsonSerializer.Serialize(messageBody, new JsonSerializerOptions().Default()));

                SendMessageResponse sendResult = await _amazonSQS.SendMessageAsync(sendRequest);

                _logger.LogInformation("MESSAGE_HAS_BEEN_SENT");

                return sendResult.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_PUBLISH_MESSAGE");
                return false;
            }
        }

        public async Task<List<T>> GetAllMessagesFromBatch<T>(SQSEvent sqsEvent)
        {
            List<T> results = new();

            foreach (SQSEvent.SQSMessage record in sqsEvent.Records)
            {
                using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(record.Body));

                T message = await JsonSerializer.DeserializeAsync<T>(memoryStream, new JsonSerializerOptions().Default());
                results.Add(message);
            }

            return results;
        }
    }
}
