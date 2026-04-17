using Demo.Consumer;
using Demo.Consumer.AdmNotificationWorker;
using Demo.Consumer.CustomerNotificationWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AdmNotificationWorker>();
builder.Services.AddHostedService<CustomerNotificationWorker>();

var host = builder.Build();
host.Run();
