using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;

namespace KPcore.ViewModels.TopicViewModels
{
    public class CreateTopicViewModel : BaseViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }

        public int SelectedSubjectId { get; set; }
    }
}