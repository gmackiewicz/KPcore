using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public void AddNotificationToMultipleUsers(string msg, List<ApplicationUser> users)
        {
            var userIds = users.Select(u => u.Id).ToList();
            AddNotificationToMultipleUsers(msg, userIds);
        }

        public void AddNotificationToMultipleUsers(string msg, List<int> userIds)
        {
            var notifId = InsertNotification(msg);

            foreach (var userId in userIds)
            {
                InsertUserNotification(userId, notifId);
            }
        }

        public void AddNotificationToUser(string msg, ApplicationUser user)
        {
            var userId = user.Id;
            AddNotificationToUser(msg, userId);
        }

        public void AddNotificationToUser(string msg, int userId)
        {
            var notifId = InsertNotification(msg);
            InsertUserNotification(userId, notifId);
        }

        private int InsertNotification(string msg)
        {
            var msgParam = new SqlParameter
            {
                ParameterName = "@Msg",
                DbType = DbType.String,
                Value = msg,
                Direction = ParameterDirection.Input
            };

            var idParam = new SqlParameter
            {
                ParameterName = "notifId",
                DbType = DbType.Int64,
                Direction = ParameterDirection.Output
            };

            var conn = _dbContext.Database.GetDbConnection();
            var command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[AddNotification]";
            command.Parameters.Add(msgParam);
            command.Parameters.Add(idParam);
            conn.Open();
            var result = (int)command.ExecuteScalar();
            conn.Close();

            return result;
        }

        private void InsertUserNotification(int userId, int notifId)
        {
            _dbContext.Database
                .ExecuteSqlCommand($"[dbo].[AddUserNotification] @UserId='{userId}', @NotifId='{notifId}'");
        }
    }
}