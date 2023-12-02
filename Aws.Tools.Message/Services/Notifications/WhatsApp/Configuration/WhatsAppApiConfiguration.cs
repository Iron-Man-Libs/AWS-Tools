using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace Aws.Tools.Message.Services.Notifications.WhatsApp.Configuration
{
    public static class WhatsAppApiConfiguration
    {
        public static void AddWhatsAppApi(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<WhatsAppConfiguration>(configuration);

            _ = services.AddHttpClient("WhatsApp", client =>
            {
                client.BaseAddress = new Uri(configuration[nameof(WhatsAppConfiguration.BaseUrl)]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration[nameof(WhatsAppConfiguration.Token)]);
            });

            _ = services.AddTransient<IWhatsAppService, WhatsAppService>();
        }
    }
}
