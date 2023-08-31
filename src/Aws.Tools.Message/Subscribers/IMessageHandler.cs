using System.Threading.Tasks;

namespace Aws.Tools.Message.Subscribers
{
    public interface IMessageHandler<T> where T : class
    {
        Task Process(T message);
    }
}
