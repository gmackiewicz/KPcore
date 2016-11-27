using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string TeacherId { get; set; }

        public ApplicationUser Teacher { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public DateTime? MeetingsDate { get; set; }
    }
}
