using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class StudentGroup
    {
        [Required]
        [StringLength(450)]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public bool Leader { get; set; }
    }
}
