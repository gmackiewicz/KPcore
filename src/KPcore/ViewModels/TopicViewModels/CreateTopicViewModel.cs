using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPcore.ViewModels.TopicViewModels
{
    public class CreateTopicViewModel : BaseViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa")]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Przedmiot")]
        public int SelectedSubjectId { get; set; }

        public List<SelectListItem> SubjectList { get; private set; }

        public CreateTopicViewModel()
        {    
        }

        public CreateTopicViewModel(IEnumerable<Subject> subjects)
        {
            SubjectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "wybierz przedmiot",
                    Value = ""
                }
            };

            foreach (var s in subjects)
            {
                SubjectList.Add(new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                });
            }
        }
    }
}