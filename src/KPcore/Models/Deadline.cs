using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models
{
    public class Deadline
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public DateTime DeadlineDate { get; set; }

        public float? Mark { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public string GetDateAndTime => DeadlineDate.Day + "." + DeadlineDate.Month + "." + DeadlineDate.Year + ", " + DeadlineDate.Hour + ":" + DeadlineDate.Minute;
    }
}
