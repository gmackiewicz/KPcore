using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.EntityFrameworkCore;

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
            return _dbContext.StudentGroups
                .Include(sg => sg.Group)
                .Where(sg => sg.StudentId == userid).ToList();
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
            return _dbContext.Groups
                .Include(g => g.Topic)
                .Include(g => g.Topic.Subject)
                .FirstOrDefault(g => g.Id == groupId);
        }

        public IEnumerable<ApplicationUser> GetStudentsOfGroup(int? groupId)
        {
            return _dbContext.StudentGroups
                .Include(g => g.Student)
                .Where(g => g.GroupId == groupId && !g.Leader)
                .Select(g => g.Student)
                .ToList();
        }

        public ApplicationUser GetLeader(int? groupId)
        {
            return _dbContext.StudentGroups
                .Include(g => g.Student)
                .Where(g => g.GroupId == groupId && g.Leader)
                .Select(g => g.Student)
                .FirstOrDefault();
        }

        public IEnumerable<GroupComment> GetGroupComments(int? groupId)
        {
            return _dbContext.GroupComments
                .Include(gc => gc.Author)
                .Where(gc => gc.GroupId == groupId).ToList();
        }

        public void AddComment(GroupComment comment)
        {
            _dbContext.GroupComments.Add(comment);
            _dbContext.SaveChanges();
        }
    }
}
