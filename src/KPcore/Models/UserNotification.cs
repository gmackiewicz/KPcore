namespace KPcore.Models
{
    public class UserNotification
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
        public bool Seen { get; set; }
    }
}