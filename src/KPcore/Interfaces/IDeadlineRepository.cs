using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface IDeadlineRepository
    {
        IEnumerable<Deadline> GetDeadlinesByGroup(int groupId);
        void AddDeadline(Deadline deadline);
    }
}
