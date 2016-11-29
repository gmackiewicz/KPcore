using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface ISubjectRepository
    {
        IEnumerable<Subject> ListAll();
        void Add(Subject subject);
    }
}
