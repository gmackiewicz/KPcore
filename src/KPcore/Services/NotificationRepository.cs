using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.EntityFrameworkCore;

namespace KPcore.Services
{
    class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Notification> GetUsersNotofications(int userId = 0)
        {
            return _dbContext.UserNotifications
                .Where(n => n.UserId == userId && n.Seen == false)
                .Include(n => n.Notification)
                .Select(n => n.Notification);
        }

        public void AddNotification(string msg, int groupId)
        {
            _dbContext.Database.ExecuteSqlCommand($"[dbo].[AddNotification] @Msg = '{msg}', @GroupId = '{groupId}'");
        }

        public bool MarkUserNotificationAsSeen(int userId, int notificationId)
        {
            var notification = _dbContext.UserNotifications
                .FirstOrDefault(n => n.UserId == userId && n.NotificationId == notificationId);

            if (notification == null)
                return false;

            notification.Seen = true;
            _dbContext.UserNotifications.Update(notification);
            _dbContext.SaveChanges();
            return true;
        }
    }
}