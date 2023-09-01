using System.Threading.Tasks;

namespace Aws.Tools.Message.SNS
{
    public interface ISNSClient
    {
        Task PublishMessageAsync<T>(string topic, T message);
    }
}
