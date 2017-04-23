using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetUsersNotofications(int userId = 0);
        bool MarkUserNotificationAsSeen(int userId, int notificationId);
        void AddNotificationToMultipleUsers(string msg, List<ApplicationUser> users);
        void AddNotificationToMultipleUsers(string msg, List<int> userIds);
        void AddNotificationToUser(string msg, ApplicationUser user);
        void AddNotificationToUser(string msg, int userId);
    }
}
