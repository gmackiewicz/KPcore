using System;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;

namespace KPcore.ViewModels.GroupViewModels
{
    public class GroupCommentViewModel : BaseViewModel
    {
        public int? CommentId { get; set; }
        public DateTime CreationDate { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Tre�� komentarza:")]
        public string Content { get; set; }
    }
}