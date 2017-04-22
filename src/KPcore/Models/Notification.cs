using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPcore.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public DateTime? NotificationDate { get; set; }

        [StringLength(200)]
        public string Content { get; set; }
    }
}