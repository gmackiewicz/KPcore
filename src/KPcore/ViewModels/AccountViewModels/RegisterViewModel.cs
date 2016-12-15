using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.AccountViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Numer indeksu")]
        public string IndexNumber { get; set; }

        [Display(Name = "Kod nauczyciela")]
        [DataType(DataType.Password)]
        public string SecretTeacherCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i co najwyżej {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }
    }
}
