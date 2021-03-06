﻿using System.Collections.Generic;
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

        public void CreateGroup(Group group, int creatorId)
        {
            _dbContext.Database.ExecuteSqlCommand($"[dbo].[CreateGroup] @Name = '{group.Name}', @UserId = '{creatorId}'");
        }

        public IEnumerable<StudentGroup> GetAllUsersGroup(int userid)
        {
            return _dbContext.StudentGroups
                .Include(sg => sg.Group)
                .OrderBy(g => g.Group.Name)
                .Where(sg => sg.StudentId == userid).ToList();
        }

        public void AddUserToGroup(int groupId, int newMemberId)
        {
            _dbContext.Database.ExecuteSqlCommand($"[dbo].[AddStudentGroup] @GroupId = '{groupId}', @StudentId = '{newMemberId}'");
        }

        public Group GetGroupById(int? groupId)
        {
            return _dbContext.Groups
                .Include(g => g.Topic)
                .Include(g => g.Topic.Subject)
                .FirstOrDefault(g => g.Id == groupId);
        }
        
        public IEnumerable<ApplicationUser> GetAllMembers(int groupId = 0)
        {
            return _dbContext.StudentGroups
                .Include(g => g.Student)
                .Where(g => g.GroupId == groupId)
                .Select(g => g.Student)
                .ToList();
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
                .Where(gc => gc.GroupId == groupId)
                .OrderByDescending(gc => gc.CreationDate)
                .ToList();
        }

        public void AddComment(GroupComment comment)
        {
            _dbContext.GroupComments.Add(comment);
            _dbContext.SaveChanges();
        }

        public void RemoveMemberFromGroup(int groupid, int memberid)
        {
            var memberToRemove = _dbContext.StudentGroups.FirstOrDefault(sg => sg.GroupId == groupid && sg.StudentId == memberid);
            _dbContext.StudentGroups.Remove(memberToRemove);
            _dbContext.SaveChanges();
        }

        public GroupComment GetCommentById(int? commentId)
        {
            return _dbContext.GroupComments
                .Include(gc => gc.Group)
                .FirstOrDefault(gc => gc.Id == commentId);
        }

        public void EditComment(GroupComment comment)
        {
            _dbContext.GroupComments.Update(comment);
            _dbContext.SaveChanges();
        }

        public void DeleteComment(int commentid)
        {
            var commentToRemove = _dbContext.GroupComments.FirstOrDefault(gc => gc.Id == commentid);
            _dbContext.GroupComments.Remove(commentToRemove);
            _dbContext.SaveChanges();
        }

        public void EditGroup(Group @group)
        {
            _dbContext.Groups.Update(group);
            _dbContext.SaveChanges();
        }

        public void DeleteGroup(int id)
        {
            var groupToRemove = _dbContext.Groups.FirstOrDefault(g => g.Id == id);
            _dbContext.Groups.Remove(groupToRemove);
            _dbContext.SaveChanges();
        }

        public void AddTopicToGroup(int groupId, int topicId)
        {
            var group = _dbContext.Groups.FirstOrDefault(g => g.Id == groupId);
            group.TopicId = topicId;
            _dbContext.Groups.Update(group);
            _dbContext.SaveChanges();
        }

        public Group GetGroupByTopicId(int? topicId)
        {
            return _dbContext.Groups.FirstOrDefault(g => g.TopicId == topicId);
        }

        public GroupComment GetLatestComment(int? groupId)
        {
            return _dbContext.GroupComments.LastOrDefault(g => g.GroupId == groupId);
        }

        public void RemoveTopicForGroup(int id)
        {
            var group = _dbContext.Groups.First(g => g.Id == id);
            group.TopicId = null;
            _dbContext.Groups.Update(group);
            _dbContext.SaveChanges();
        }
    }
}
