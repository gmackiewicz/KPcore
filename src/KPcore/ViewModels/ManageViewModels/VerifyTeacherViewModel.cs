using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.ManageViewModels
{
    public class VerifyTeacherViewModel : BaseViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Code { get; set; }
    }
}
