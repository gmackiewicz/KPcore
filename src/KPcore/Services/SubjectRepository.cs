using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.EntityFrameworkCore;

namespace KPcore.Services
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Subject> ListAll()
        {
            return _dbContext.Subjects.AsEnumerable();
        }

        public Subject FindSubjectById(int? subjectId)
        {
            return _dbContext.Subjects
                .FirstOrDefault(sid => sid.Id == subjectId);
        }
        public void Add(Subject subject)
        {
            _dbContext.Subjects.Add(subject);
            _dbContext.SaveChanges();
        }

        public void Remove(Subject subject)
        {
            _dbContext.Subjects.Remove(subject);
            _dbContext.SaveChanges();
        }
    }
}
