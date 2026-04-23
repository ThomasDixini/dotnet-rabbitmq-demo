namespace Demo.API.Requests
{
    public class ScheduleRequest
    {
        public int ScheduleId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Procedure { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
    }
}