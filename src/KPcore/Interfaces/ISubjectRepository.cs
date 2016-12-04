using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface ISubjectRepository
    {
        IEnumerable<Subject> ListAll();
        void AddSubject(Subject subject);
        void RemoveSubject(Subject subject);
        Subject FindSubjectById(int? subjectId);
        void EditSubject(Subject subject);
    }
}
