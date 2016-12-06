using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class StudentGroup : IComparable
    {
        [Required]
        [StringLength(450)]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public bool Leader { get; set; }
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is StudentGroup)) return 1;
            else return this.Group.Name.CompareTo(((StudentGroup) obj).Group.Name);
        }
    }
}
