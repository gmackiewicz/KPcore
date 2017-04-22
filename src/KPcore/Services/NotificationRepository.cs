using System.Collections.Generic;
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
                .Where(n => n.UserId == userId)
                .Include(n => n.Notification)
                .Select(n => n.Notification);
        }
    }
}