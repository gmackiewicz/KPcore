using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;

namespace KPcore.Services
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateGroup(Group group, string creatorId)
        {
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
            AddUserToGroup(group.Id, creatorId, true);
        }

        public IEnumerable<StudentGroup> GetAllUsersGroup(string userid)
        {
            var studentGroups = _dbContext.StudentGroups.Where(sg => sg.StudentId == userid);

            return (from sg in studentGroups
                    join Group g in _dbContext.Groups
                    on sg.GroupId equals g.Id
                    select new StudentGroup
                    {
                        GroupId = g.Id,
                        Group = g,
                        Leader = sg.Leader,
                        StudentId = sg.StudentId
                    }
            ).ToList();
        }

        public void AddUserToGroup(int groupId, string newMemberId, bool leader)
        {
            var studentGroup = new StudentGroup
            {
                GroupId = groupId,
                StudentId = newMemberId,
                Leader = leader
            };

            _dbContext.StudentGroups.Add(studentGroup);
            _dbContext.SaveChanges();
        }

        public Group GetGroupById(int? groupId)
        {
            return _dbContext.Groups.FirstOrDefault(g => g.Id == groupId);
        }
    }
}
