using System;
using KPcore.Models;

namespace KPcore.ViewModels.TopicViewModels
{
    public class TopicDetailsViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApplicationUser Teacher { get; set; }
        public Subject Subject { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? MeetingsDate { get; set; }
    }
}