using Microsoft.Extensions.Options;
using System;

namespace Aws.Tools.Message.Services.Messages.MessagesServicePath
{
    public class MessagePath<T> : IMessagePath<T> where T : Enum
    {
        private readonly string _baseSNSARN;

        private readonly string _baseSQSURL;

        public MessagePath(IOptions<MessageConfiguration> messageConfiguration)
        {
            string region = messageConfiguration.Value.Region;

            string accountId = messageConfiguration.Value.AccountId;

            _baseSNSARN = $"arn:aws:sns:{region}:{accountId}:";
            _baseSQSURL = $"https://sqs.{region}.amazonaws.com/{accountId}/";
        }


        public string GetSNSARN(T queueName)
        {
            return _baseSNSARN + queueName.ToString();
        }

        public string GetSQSQueueUrl(T queueName, string applicationName = null)
        {
            return !string.IsNullOrEmpty(applicationName) ? _baseSQSURL + queueName + applicationName.Trim() : _baseSQSURL + queueName.ToString();
        }
    }
}
