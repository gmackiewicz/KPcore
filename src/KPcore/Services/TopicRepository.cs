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

        public IEnumerable<TopicEntry> GetTopicComments(int? topicId)
        {
            return _dbContext.TopicEntries
                .Include(te => te.Author)
                .Where(te => te.TopicId == topicId)
                .OrderByDescending(te => te.CreationDate);
        }

        public void AddComment(TopicEntry comment)
        {
            _dbContext.TopicEntries.Add(comment);
            _dbContext.SaveChanges();
        }

        public void EditComment(TopicEntry comment)
        {
            _dbContext.TopicEntries.Update(comment);
            _dbContext.SaveChanges();
        }

        public void DeleteComment(int commentid)
        {
            var commentToRemove = _dbContext.TopicEntries.FirstOrDefault(gc => gc.Id == commentid);
            _dbContext.TopicEntries.Remove(commentToRemove);
            _dbContext.SaveChanges();
        }

        public TopicEntry GetCommentById(int? commentId)
        {
            return _dbContext.TopicEntries
                .Include(te => te.Author)
                .Include(te => te.Topic)
                .FirstOrDefault(te => te.Id == commentId);
        }
    }
}
