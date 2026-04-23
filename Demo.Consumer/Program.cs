using Demo.Consumer.AdmNotificationWorker;
using Demo.Consumer.CustomerNotificationWorker;
using Demo.Consumer.Handlers;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((services) =>
{
    services.AddHttpClient<ConfirmatedScheduleHandler>();
    services.AddHttpClient<CanceledScheduleHandler>();
    services.AddHostedService<AdmNotificationWorker>();
    services.AddHostedService<CustomerNotificationWorker>();
});

var host = builder.Build();
host.Run();
