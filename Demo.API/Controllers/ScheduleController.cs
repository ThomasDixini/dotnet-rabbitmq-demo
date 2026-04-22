using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.API.Requests;
using Demo.Contracts.Events.Interfaces;
using Demo.Contracts.Queues.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(IEventPublisher eventPublisher, ILogger<ScheduleController> logger)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> Schedule([FromBody] ScheduleRequest request)
        {
            var task = new {
                ScheduleId = request.ScheduleId,
                CustomerName = request.CustomerName,
                Procedure = request.Procedure,
                ScheduleDate = request.ScheduleDate
            };

            await _eventPublisher.PublishAsync(task, ExchangeNames.NotificationsExchange, RoutingKeys.CustomerNotification);
            _logger.LogInformation("Schedule event published for ScheduleId: {ScheduleId}", request.ScheduleId);

            await _eventPublisher.PublishAsync(task, ExchangeNames.NotificationsExchange, RoutingKeys.AdmNotification);
            _logger.LogInformation("Schedule event published for ScheduleId: {ScheduleId}", request.ScheduleId);

            return Ok(new
            {
                Message = "Schedule created and notifications published successfully.",
                ScheduleId = request.ScheduleId,
                Queues = new[] { QueueNames.CustomerNotificationQueue, QueueNames.AdmNotificationQueue }
            });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> CancelSchedule(int id, [FromBody] CancelScheduleRequest request)
        {
            var task = new {
                CustomerName = request.CustomerName,
                Procedure = request.Procedure,
                ScheduleDate = request.ScheduleDate,
                Token = request.Token
            };

            await _eventPublisher.PublishAsync(task, ExchangeNames.NotificationsExchange, RoutingKeys.CustomerNotification);
            _logger.LogInformation("Schedule event published for ScheduleId: {ScheduleId}", id);

            await _eventPublisher.PublishAsync(task, ExchangeNames.NotificationsExchange, RoutingKeys.AdmNotification);
            _logger.LogInformation("Schedule event published for ScheduleId: {ScheduleId}", id);

            return Ok(new
            {
                Message = "Schedule canceled and notifications published successfully.",
                ScheduleId = id,
                Queues = new[] { QueueNames.CustomerNotificationQueue, QueueNames.AdmNotificationQueue }
            });
        }
    }
}