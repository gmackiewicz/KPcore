using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.ManageViewModels
{
    public class VerifyPhoneNumberViewModel : BaseViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
