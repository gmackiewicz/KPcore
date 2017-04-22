using System;

namespace KPcore.ViewModels.TopicViewModels
{
    public class EditTopicViewModel : BaseViewModel
    {
        public int TopicId { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? MeetingsDate { get; set; }
        public int TeacherId { get; set; }
    }
}