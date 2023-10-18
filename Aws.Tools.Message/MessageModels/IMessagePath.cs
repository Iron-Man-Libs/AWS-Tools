using System;

namespace Aws.Tools.Message.MessageModels
{
    public interface IMessagePath<T> where T : Enum
    {
        string GetSNSARN(T messageType);

        public string GetSQSQueueUrl(T queueName, string applicationName = null);
    }
}
