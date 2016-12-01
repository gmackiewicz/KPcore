using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.GroupViewModels
{
    public class CreateGroupViewModel : BaseViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}