using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.EntityFrameworkCore;

namespace KPcore.Services
{
    public class DeadlineRepository : IDeadlineRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeadlineRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Deadline> GetDeadlinesByGroup(int groupId)
        {
            return _dbContext.Deadlines.Where(d => d.GroupId == groupId).ToList();
        }

        public void AddDeadline(Deadline deadline)
        {
            _dbContext.Deadlines.Add(deadline);
            _dbContext.SaveChanges();
        }

        public Deadline GetCurrentDeadline(int groupId)
        {
            return _dbContext.Deadlines.First(d => d.DeadlineDate > DateTime.Now);
        }

        public Deadline GetDeadlineById(int id)
        {
            return _dbContext.Deadlines
                .Include(d => d.Group)
                .Include(d => d.Group.Topic)
                .FirstOrDefault(d => d.Id == id);
        }

        public void UpdateDeadline(Deadline deadline)
        {
            _dbContext.Deadlines.Update(deadline);
            _dbContext.SaveChanges();
        }
    }
}
