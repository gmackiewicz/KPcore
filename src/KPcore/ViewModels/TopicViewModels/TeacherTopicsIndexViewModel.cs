using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.ViewModels.TopicViewModels
{
    public class TeacherTopicsIndexViewModel : BaseViewModel
    {
        public IEnumerable<Topic> Topics { get; set; }
    }
}