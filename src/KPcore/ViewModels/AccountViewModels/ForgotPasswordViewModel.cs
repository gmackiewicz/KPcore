using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
