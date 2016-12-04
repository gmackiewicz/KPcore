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
    }
}
