using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.EntityFrameworkCore;

namespace KPcore.Services
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateTopic(Topic topic)
        {
            _dbContext.Topics.Add(topic);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Topic> GetAllUsersTopics(string userid)
        {
            return _dbContext.Topics
                .Include(t => t.Subject)
                .Where(t => t.TeacherId == userid);
        }

        public Topic GetTopicById(int? topicId)
        {
            return _dbContext.Topics
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .FirstOrDefault(t => t.Id == topicId);
        }

        public IEnumerable<Topic> GetAvailableTopics()
        {
            var usedTopics = _dbContext.Groups.Select(g => g.TopicId).ToList();

            return _dbContext.Topics
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .Where(t => !usedTopics.Contains(t.Id));
        }
        public void DeleteTopic(int topicid)
        {
            var topicToRemove = _dbContext.Topics.FirstOrDefault(gc => gc.Id == topicid);
            _dbContext.Topics.Remove(topicToRemove);
            _dbContext.SaveChanges();
        }
    }
}
