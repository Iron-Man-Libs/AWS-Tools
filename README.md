# AWS Tools 
> This is a nuget package, which provides a brief implementation of AWS services in order to provide an abstraction.

# How To Use
> This is an extension class that you need to extend all services that you want. The only thing that you need to do is **services.AddNotifications** or using another name of your preference within Program.cs or Startup.cs.

    public static class NotificationDependency
    {
        public static void AddNotifications(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            services.AddAWSService<IAmazonSimpleNotificationService>();
            services.AddSingleton<ISQSClient, SQSClient>();
            services.AddSingleton<ISNSClient, SNSClient>();
        }
    }


