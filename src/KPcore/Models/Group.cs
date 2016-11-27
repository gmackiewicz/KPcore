using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class Group
    {
        public int Id { get; set; }

        public int? SubjectId { get; set; }

        public Subject Subject { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
