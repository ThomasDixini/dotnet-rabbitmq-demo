using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Consumer.AdmNotificationWorker
{
    public class AdmNotificationWorker(ILogger<AdmNotificationWorker> logger) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("AdmNotificationWorker is running.");
                Task.Delay(1000, stoppingToken).Wait();
            }
            return Task.CompletedTask;
        }
    }
}