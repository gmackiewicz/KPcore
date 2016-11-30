using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.ViewModels.StudentGroupsViewModels
{
    public class StudentGroupIndexViewModel : BaseViewModel
    {
        public IEnumerable<StudentGroup> StudentGroups { get; set; }
    }
}
