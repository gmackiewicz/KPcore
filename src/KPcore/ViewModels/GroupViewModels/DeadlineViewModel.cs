using System;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;

namespace KPcore.ViewModels.GroupViewModels
{
    public class DeadlineViewModel : BaseViewModel
    {
        public Group Group { get; set; }

        public int GroupId { get; set; }
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Data terminu")]
        public DateTime? DeadlineDate { get; set; } = null;
        
        [Display(Name = "Komentarz")]
        [StringLength(500)]
        public string Comment { get; set; }
    }
}