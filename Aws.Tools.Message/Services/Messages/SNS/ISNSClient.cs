using System.Threading.Tasks;

namespace Aws.Tools.Message.Services.Messages.SNS
{
    public interface ISNSClient
    {
        Task PublishMessageAsync<T>(string topic, T message, string entityName = null, string apiName = null);
    }
}
