using System;
using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.GroupViewModels
{
    public class MarkDeadlineViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int GroupId { get; set; }

        [Display(Name = "Ocena")]
        [Range(2.0, 5.0)]
        public string Mark { get; set; }

        [Display(Name = "Komentarz")]
        [StringLength(500)]
        public string Comment { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string DateAndTime { get; set; }
    }
}