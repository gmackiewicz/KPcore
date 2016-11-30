using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;

namespace KPcore.Services
{
    public class StudentGroupRepository : IStudentGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentGroupRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<StudentGroup> GetAllUsersGroup(string userid)
        {
            return _dbContext.StudentGroups.Where(sg => sg.StudentId == userid);
        }
        

        public void Add(StudentGroup studentGroup)
        {
            _dbContext.StudentGroups.Add(studentGroup);
            _dbContext.SaveChanges();
        }
    }
}
