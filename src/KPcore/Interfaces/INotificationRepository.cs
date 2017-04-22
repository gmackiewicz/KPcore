using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetUsersNotofications(int userId = 0);
    }
}
