using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface IGroupRepository
    {
        void CreateGroup(Group group, string creator);
        IEnumerable<StudentGroup> GetAllUsersGroup(string userid);
        void AddUserToGroup(int groupId, string newMemberId, bool leader);
        Group GetGroupById(int? groupId);
        IEnumerable<ApplicationUser> GetStudentsOfGroup(int? groupId);
        ApplicationUser GetLeader(int? groupId);
        IEnumerable<GroupComment> GetGroupComments(int? groupId);
        void AddComment(GroupComment comment);
        void RemoveMemberFromGroup(int groupid, string memberid);
        GroupComment GetCommentById(int? commentId);
        void EditComment(GroupComment comment);
        void DeleteComment(int commentid);
        void EditGroup(Group group);
        void DeleteGroup(int id);
    }
}
