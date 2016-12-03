using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetAllStudents();
    }
}
