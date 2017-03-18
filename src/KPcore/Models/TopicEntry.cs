using System;
using System.ComponentModel.DataAnnotations;

namespace KPcore.Models
{
    public class TopicEntry
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public Topic Topic { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }
    }
}
