using System.Text;
using Demo.Contracts.Events;
namespace Demo.Consumer.Handlers
{
    public class ConfirmatedScheduleHandler(ILogger<ConfirmatedScheduleHandler> _logger, HttpClient _http)
    {
        public async Task HandleAdmScheduleConfirmation(ConfirmatedScheduleEvent body)
        {
            var message = $"Schedule confirmed: {body.ScheduleId} - {body.CustomerName} - {body.ScheduleDate} - {body.Procedure}";
            _logger.LogInformation(message);
            var mensagem = new
            {
                to = body.Token,
                title = "Schedule Confirmation",
                body = message,
                data = new { type = "event" },
            };

            var json = System.Text.Json.JsonSerializer.Serialize(mensagem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://exp.host/--/api/v2/push/send", content);
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Push notification response: {result}");
        }
        public async Task HandleCustomerScheduleConfirmation(ConfirmatedScheduleEvent body)
        {
            var message = $"Schedule confirmed: {body.Procedure} - {body.ScheduleDate}";
            _logger.LogInformation(message);
            var eventMessage = new
            {
                to = body.Token,
                title = "Schedule Confirmation",
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