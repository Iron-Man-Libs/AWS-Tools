using Microsoft.Extensions.Options;
using System;

namespace Aws.Tools.Message.MessageModels
{
    public class MessagePath<T> : IMessagePath<T> where T : Enum
    {
        private readonly string _baseSNSARN;

        private readonly string _baseSQSURL;

        public MessagePath(IOptions<MessageConfiguration> messageConfiguration)
        {
            var region = messageConfiguration.Value.Region;

            var accountId = messageConfiguration.Value.AccountId;

            _baseSNSARN = $"arn:aws:sns:{region}:{accountId}:";
            _baseSQSURL = $"https://sqs.{region}.amazonaws.com/{accountId}/";
        }


        public string GetSNSARN(T queueName)
        {
            return _baseSNSARN + queueName.ToString();
        }

        public string GetSQSQueueUrl(T queueName, string applicationName = null)
        {
            if (!string.IsNullOrEmpty(applicationName))
            {
                return _baseSQSURL + queueName + applicationName.Trim();
            }

            return _baseSQSURL + queueName.ToString();
        }
    }
}
