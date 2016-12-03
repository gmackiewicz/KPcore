using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface ITopicRepository
    {
        void CreateTopic(Topic topic);
        IEnumerable<Topic> GetAllUsersTopics(string userid);
        Topic GetTopicById(int? topicId);
        IEnumerable<Topic> GetAvailableTopics();
    }
}
