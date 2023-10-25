using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.Subscribers
{
    public interface ISubscriber<T> where T : class
    {
        Task Subscribe(string queueName);

        Task ProcessMessage(string message);
    }
}
