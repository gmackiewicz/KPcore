using System.Collections.Generic;
using KPcore.Models;

namespace KPcore.Interfaces
{
    public interface IGroupRepository
    {
        void CreateGroup(Group group, int creator);
        IEnumerable<StudentGroup> GetAllUsersGroup(int userid);
        void AddUserToGroup(int groupId, int newMemberId, bool leader);
        Group GetGroupById(int? groupId);
        IEnumerable<ApplicationUser> GetStudentsOfGroup(int? groupId);
        ApplicationUser GetLeader(int? groupId);
        IEnumerable<GroupComment> GetGroupComments(int? groupId);
        void AddComment(GroupComment comment);
        void RemoveMemberFromGroup(int groupid, int memberid);
        GroupComment GetCommentById(int? commentId);
        void EditComment(GroupComment comment);
        void DeleteComment(int commentid);
        void EditGroup(Group group);
        void DeleteGroup(int id);
        void AddTopicToGroup(int modelGroupId, int modelSelectedTopic);
        Group GetGroupByTopicId(int? topicId);
        GroupComment GetLatestComment(int? groupId);
        void RemoveTopicForGroup(int id);
    }
}
