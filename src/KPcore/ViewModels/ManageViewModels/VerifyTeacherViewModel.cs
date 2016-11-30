using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.ManageViewModels
{
    public class VerifyTeacherViewModel : BaseViewModel
    {
        [Required]
        public string Code { get; set; }
    }
}
