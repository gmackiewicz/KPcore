using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.ViewModels.GroupViewModels
{
    public class StudentGroupIndexViewModel : BaseViewModel
    {
        public IEnumerable<StudentGroup> StudentGroups { get; set; }
    }
}
