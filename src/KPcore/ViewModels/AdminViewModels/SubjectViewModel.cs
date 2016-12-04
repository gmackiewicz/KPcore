using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPcore.ViewModels.AdminViewModels
{
    public class SubjectViewModel : BaseViewModel
    {
        public int? SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa przedmiotu")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}
