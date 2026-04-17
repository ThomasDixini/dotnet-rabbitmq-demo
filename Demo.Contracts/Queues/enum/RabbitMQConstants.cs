namespace Demo.Contracts.Queues.Enum
{
    public static class QueueNames
    {
       public const string AdmNotificationQueue = "adm.notification.queue";
       public const string CustomerNotificationQueue = "customer.notification.queue";
    }
    public static class ExchangeNames
    {
       public const string NotificationsExchange = "notifications.exchange";
    }
    public static class RoutingKeys
    {
       public const string AdmNotification = "adm.notification";
       public const string CustomerNotification = "customer.notification";
    }
}