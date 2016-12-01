﻿using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;

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
            return _dbContext.Topics.Where(t => t.TeacherId == userid);
        }
    }
}