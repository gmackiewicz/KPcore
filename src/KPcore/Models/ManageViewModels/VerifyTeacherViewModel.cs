using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.Models.ManageViewModels
{
    public class VerifyTeacherViewModel
    {
        [Required]
        public string Code { get; set; }
    }
}
