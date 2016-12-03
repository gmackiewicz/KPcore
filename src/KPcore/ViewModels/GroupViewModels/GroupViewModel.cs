using System.ComponentModel.DataAnnotations;

namespace KPcore.ViewModels.GroupViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int? TopicId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}