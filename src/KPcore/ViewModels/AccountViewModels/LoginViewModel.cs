using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.AccountViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać dane logowania?")]
        public bool RememberMe { get; set; }
    }
}
