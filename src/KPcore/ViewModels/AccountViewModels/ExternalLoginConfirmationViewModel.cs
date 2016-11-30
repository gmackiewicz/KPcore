using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
