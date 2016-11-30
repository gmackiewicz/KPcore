using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface IStudentGroupRepository
    {
        IEnumerable<StudentGroup> GetAllUsersGroup(string userid);
        void Add(StudentGroup studentGroup);
    }
}
