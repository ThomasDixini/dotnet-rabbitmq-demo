using Demo.Consumer;
using Demo.Consumer.AdmNotificationWorker;
using Demo.Consumer.CustomerNotificationWorker;
using Demo.Consumer.Handlers;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((services) =>
{
    services.AddTransient<CanceledScheduleHandler>();
    services.AddTransient<ConfirmatedScheduleHandler>();
    services.AddHttpClient();
    services.AddHostedService<AdmNotificationWorker>();
    services.AddHostedService<CustomerNotificationWorker>();
});

var host = builder.Build();
host.Run();
