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

        public int? TopicId { get; set; }

        public Topic Topic { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
