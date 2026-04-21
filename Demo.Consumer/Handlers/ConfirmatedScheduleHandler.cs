using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Contracts.Events;

namespace Demo.Consumer.Handlers
{
    public class ConfirmatedScheduleHandler(ILogger<ConfirmatedScheduleHandler> _logger)
    {
        public async Task HandleAdmScheduleConfirmation(ConfirmatedScheduleEvent body)
        {
            _logger.LogInformation($"Schedule confirmed: {body.ScheduleId} - {body.CustomerName} - {body.ScheduleDate} - {body.Procedure}");
            await Task.CompletedTask;
        }
        public async Task HandleCustomerScheduleConfirmation(ConfirmatedScheduleEvent body)
        {
            _logger.LogInformation($"Schedule confirmed: {body.Procedure} - {body.ScheduleDate}");
            await Task.CompletedTask;
        }
    }
}