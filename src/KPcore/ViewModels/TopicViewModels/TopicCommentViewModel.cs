using System;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;

namespace KPcore.ViewModels.TopicViewModels
{
    public class TopicCommentViewModel : BaseViewModel
    {
        public int? CommentId { get; set; }
        public DateTime CreationDate { get; set; }
        public Topic Topic { get; set; }
        public int TopicId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Treść komentarza:")]
        public string Content { get; set; }
    }
}