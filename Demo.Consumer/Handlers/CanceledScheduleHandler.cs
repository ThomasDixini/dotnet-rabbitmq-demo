using System.Text;
using Demo.Contracts.Events;
namespace Demo.Consumer.Handlers
{
    public class CanceledScheduleHandler(ILogger<CanceledScheduleHandler> _logger, HttpClient _http)
    {
        public async Task HandleAdmScheduleCancellation(CanceledScheduleEvent body)
        {
            var message = $"Schedule canceled: {body.ScheduleId} - {body.CustomerName} - {body.ScheduleDate} - {body.Procedure}";
            _logger.LogInformation(message);
            var eventMessage = new
            {
                to = body.Token,
                title = "Schedule Cancellation",
                body = message,
                data = new { type = "event" },
            };

            var json = System.Text.Json.JsonSerializer.Serialize(eventMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://exp.host/--/api/v2/push/send", content);
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Push notification response: {result}");
        }
        public async Task HandleCustomerScheduleCancellation(CanceledScheduleEvent body)
        {
            var message = $"Schedule canceled: {body.Procedure} - {body.ScheduleDate}";
            _logger.LogInformation(message);
            var eventMessage = new
            {
                to = body.Token,
                title = "Schedule Cancellation",
                body = message,
                data = new { type = "event" },
            };

            var json = System.Text.Json.JsonSerializer.Serialize(eventMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://exp.host/--/api/v2/push/send", content);
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Push notification response: {result}");
        }
    }
}