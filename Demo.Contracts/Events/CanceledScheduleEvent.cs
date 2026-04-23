namespace Demo.Contracts.Events
{
    public record CanceledScheduleEvent
    {
        public int ScheduleId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public DateTime ScheduleDate { get; init; }
        public string Procedure { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}