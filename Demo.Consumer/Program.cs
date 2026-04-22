using Demo.Consumer;
using Demo.Consumer.AdmNotificationWorker;
using Demo.Consumer.CustomerNotificationWorker;
using Demo.Consumer.Handlers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<CanceledScheduleHandler>();
builder.Services.AddSingleton<ConfirmatedScheduleHandler>();
builder.Services.AddHostedService<AdmNotificationWorker>();
builder.Services.AddHostedService<CustomerNotificationWorker>();

var host = builder.Build();
host.Run();
