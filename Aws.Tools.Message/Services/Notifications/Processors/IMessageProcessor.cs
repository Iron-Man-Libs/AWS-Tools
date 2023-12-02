using System.Threading.Tasks;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Aws.Tools.Message.Services.Messages.Processors
{
    public interface IMessageProcessor<T> where T : class
    {
        Task Process(SQSMessage message);
    }
}