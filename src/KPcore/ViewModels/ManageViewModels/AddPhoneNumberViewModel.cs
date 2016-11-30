using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.ManageViewModels
{
    public class AddPhoneNumberViewModel : BaseViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
