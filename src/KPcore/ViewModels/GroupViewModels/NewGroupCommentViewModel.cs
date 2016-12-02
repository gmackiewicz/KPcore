using System.ComponentModel.DataAnnotations;
using KPcore.Models;

namespace KPcore.ViewModels.GroupViewModels
{
    public class NewGroupCommentViewModel : BaseViewModel
    {
        public Group Group { get; set; }
        public int GroupId { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }
    }
}