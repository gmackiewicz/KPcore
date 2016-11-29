using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.ManageViewModels
{
    public class VerifyTeacherViewModel
    {
        [Required]
        public string Code { get; set; }
    }
}
