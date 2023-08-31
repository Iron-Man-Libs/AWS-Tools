using System.Threading.Tasks;

namespace Aws.Tools.Message.Subscribers
{
    public interface ISubscriber<T> where T : class
    {
        Task Subscribe(string queueName);
    }
}
