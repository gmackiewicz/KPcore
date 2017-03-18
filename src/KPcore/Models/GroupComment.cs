using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class GroupComment
    {
        public int Id { get; set; }

        public int GroupId { get; set; }


        public Group Group { get; set; }

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
