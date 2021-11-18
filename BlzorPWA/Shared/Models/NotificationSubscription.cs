#nullable disable

namespace BlzorPWA.Shared.Models
{
    public partial class NotificationSubscription
    {
        public int NotificationSubscriptionId { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
