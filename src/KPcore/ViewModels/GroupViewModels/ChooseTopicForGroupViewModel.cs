using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using KPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPcore.ViewModels.GroupViewModels
{
    public class ChooseTopicForGroupViewModel : BaseViewModel
    {
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public IEnumerable<Topic> Topics { get; set; }

        public List<SelectListItem> TopicsList { get; private set; }

        [Required]
        [Display(Name = "Wybrany temat")]
        public int SelectedTopic { get; set; }

        public ChooseTopicForGroupViewModel() { }

        public ChooseTopicForGroupViewModel(IEnumerable<Topic> availableTopics)
        {
            var list = availableTopics as IList<Topic> ?? availableTopics.ToList();
            Topics = list;

            TopicsList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "wybierz przedmiot",
                    Value = ""
                }
            };

            foreach (var t in availableTopics)
            {
                TopicsList.Add(new SelectListItem
                {
                    Text = "[" + t.Id + "] " + t.Title,
                    Value = t.Id.ToString()
                });
            }
        }
    }
}