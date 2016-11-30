using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.ViewModels.AdminViewModels
{
    public class AddSubjectViewModel : BaseViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Subject name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
