using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Notifications.Processors
{
    public interface IMessageHandler<T> where T : class
    {
        Task Process(T message);
    }
}
