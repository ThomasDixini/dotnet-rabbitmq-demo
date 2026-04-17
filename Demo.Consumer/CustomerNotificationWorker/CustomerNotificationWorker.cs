using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Consumer.CustomerNotificationWorker
{
    public class CustomerNotificationWorker(ILogger<CustomerNotificationWorker> logger) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("CustomerNotificationWorker is running.");
                Task.Delay(1000, stoppingToken).Wait();
            }
            return Task.CompletedTask;
        }
    }
}