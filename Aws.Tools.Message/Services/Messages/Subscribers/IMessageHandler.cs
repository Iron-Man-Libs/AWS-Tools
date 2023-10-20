using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.Subscribers
{
    public interface IMessageHandler<T> where T : class
    {
        Task Process(T message);
    }
}
