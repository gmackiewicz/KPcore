using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
using KPcore.ViewModels.GroupViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace KPcore.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IDeadlineRepository _deadlineRepository;

        public GroupController(UserManager<ApplicationUser> userManager,
            IGroupRepository groupRepository,
            ITopicRepository topicRepository,
            IDeadlineRepository deadlineRepository,
            IUserRepository userRepository) : base(userManager)
        {
            _groupRepository = groupRepository;
            _topicRepository = topicRepository;
            _deadlineRepository = deadlineRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GroupMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == GroupMessageId.CreateGroupSuccess ? "Nowa grupa została utworzona."
                : message == GroupMessageId.NoGroupToView ? "Nie ma takiej grupy."
                : message == GroupMessageId.ErrorAddingCommentToGroup ? "Błąd podczas dodawania nowego komentarza."
                : message == GroupMessageId.LeaveGroupSuccess ? "Opuściłeś grupę."
                : message == GroupMessageId.DeleteGroupSuccess ? "Usunąłeś grupę."
                : message == GroupMessageId.Error ? "Wystąpił błąd."
                : message == GroupMessageId.NameChanged ? "Pomyślnie zmieniono nazwę grupy."
                : message == GroupMessageId.NameChangeError ? "Wystąpił błąd podczas zmieniana nazwy grupy: nieprawidłowa nazwa."
                : "";

            var user = await GetCurrentUserAsync();
            var _StudentGroups = _groupRepository.GetAllUsersGroup(user.Id);
            Dictionary<StudentGroup, GroupComment> dict = new Dictionary<StudentGroup, GroupComment>();
            foreach (var group in _StudentGroups)
            {
                var comment = _groupRepository.GetLatestComment(group.GroupId) ?? new GroupComment
                {
                    Content = "Brak"
                };
                dict.Add(group, comment);
            }
            var model = new StudentGroupIndexViewModel
            {
                StudentGroups = dict
            };

            return View(model);
        }

        // GET: /Group/Create
        public IActionResult Create()
        {
            return View(new GroupViewModel());
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się stworzyć nowej grupy.");
                return View(model);
            }

            var group = new Group
            {
                Name = model.Name
            };

            _groupRepository.CreateGroup(group, user.Id);
            return RedirectToAction(nameof(Index), new { Message = GroupMessageId.CreateGroupSuccess });
        }

        // GET: /Group/Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.NoGroupToView });
            }

            var group = _groupRepository.GetGroupById(id);

            if (group == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.NoGroupToView });
            }

            var studentsList = _groupRepository.GetStudentsOfGroup(id);

            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                TopicId = group.TopicId,
                Topic = group.Topic,
                StudentsList = studentsList,
                GroupLeader = _groupRepository.GetLeader(id),
                GroupComments = _groupRepository.GetGroupComments(id)
            };

            return View(model);
        }

        public async Task<IActionResult> EditGroupName(GroupDetailsViewModel model)
        {
            var user = await GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { message = GroupMessageId.NameChangeError });
            }
            if (user.Status == 2)
            {
                var group = _groupRepository.GetGroupById(model.Id);
                group.Name = model.Name;
                _groupRepository.EditGroup(group);
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.NameChanged });
            }
            return RedirectToAction(nameof(Index), new { message = GroupMessageId.Error });
        }

        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = _groupRepository.GetGroupById(id);
            var user = await GetCurrentUserAsync();

            var groupLeader = _groupRepository.GetLeader(id);
            if (user != null && groupLeader.Id == user.Id)
            {
                _groupRepository.DeleteGroup(id);
            }
            return RedirectToAction(nameof(Index), new { Message = GroupMessageId.DeleteGroupSuccess });
        }

        #region GroupComments 

        // GET: /Group/Comment
        public IActionResult Comment(int groupid, int? commentid)
        {
            var group = _groupRepository.GetGroupById(groupid);
            if (group == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.ErrorAddingCommentToGroup });
            }
            var model = new GroupCommentViewModel
            {
                Group = @group,
                GroupId = groupid
            };

            var comment = _groupRepository.GetCommentById(commentid);
            if (comment != null)
            {
                model.CommentId = commentid;
                model.Content = comment.Content;
                model.CreationDate = comment.CreationDate;
                ViewData["Title"] = "Edytuj komentarz";
            }
            else
            {
                ViewData["Title"] = "Dodaj komentarz";
            }

            return View(model);
        }

        // POST: /Group/Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(GroupCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Akcja zakończyła się niepowodzeniem.");
                return View(model);
            }

            var comment = new GroupComment
            {
                GroupId = model.GroupId,
                AuthorId = user.Id,
                Content = model.Content,
            };

            if (model.CommentId == null)
            {
                comment.CreationDate = DateTime.Now;
                _groupRepository.AddComment(comment);
            }
            else
            {
                comment.Id = model.CommentId.Value;
                comment.CreationDate = model.CreationDate;
                comment.ModificationDate = DateTime.Now;
                _groupRepository.EditComment(comment);
            }

            return RedirectToAction(nameof(Details), new { id = comment.GroupId });
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _groupRepository.GetCommentById(id);
            var user = await GetCurrentUserAsync();

            if (comment.AuthorId == user.Id)
            {
                _groupRepository.DeleteComment(id);
            }
            return RedirectToAction(nameof(Details), new { id = comment.GroupId });
        }

        #endregion

        #region GroupMembers

        public async Task<IActionResult> RemoveMember(int groupid, string memberid)
        {
            var currentUser = await GetCurrentUserAsync();
            var groupLeader = _groupRepository.GetLeader(groupid);

            if (currentUser != null && currentUser.Id == groupLeader.Id)
            {
                _groupRepository.RemoveMemberFromGroup(groupid, memberid);
                return RedirectToAction(nameof(Details), new { id = groupid });
            }

            ModelState.AddModelError(string.Empty, "Nie udało się usunąć członka grupy.");
            return RedirectToAction(nameof(Details), new { id = groupid });
        }

        public async Task<IActionResult> AddMember(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            var groupLeader = _groupRepository.GetLeader(id);

            if (currentUser == null || currentUser.Id != groupLeader.Id)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            var students = _userRepository.GetAllStudents();
            var groupMembers = _groupRepository.GetStudentsOfGroup(id).Select(gm => gm.Id);
            var leader = _groupRepository.GetLeader(id);
            var membersToAdd = students.Where(s => (!groupMembers.Contains(s.Id)) && leader.Id != s.Id).ToList();

            var model = new AddMemberToGroupViewModel(membersToAdd) { GroupId = id };
            ViewBag.UserList = model.UsersList;

            return View(model);
        }

        // POST: /Group/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(AddMemberToGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się dodać użytkownika do grupy.");
                return View(model);
            }

            _groupRepository.AddUserToGroup(model.GroupId, model.SelectedUser, false);
            return RedirectToAction(nameof(Details), new { id = model.GroupId });
        }

        public async Task<IActionResult> LeaveGroup(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                _groupRepository.RemoveMemberFromGroup(id, user.Id);
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.LeaveGroupSuccess });
            }
            return RedirectToAction(nameof(Index), new { Message = GroupMessageId.Error });
        }

        #endregion

        #region GroupTopic

        // GET: /Group/ChooseTopicForGroup
        public IActionResult ChooseTopicForGroup(int id)
        {
            var group = _groupRepository.GetGroupById(id);
            var availableTopics = _topicRepository.GetAvailableTopics();

            if (group != null && group.TopicId == null)
            {
                var model = new ChooseTopicForGroupViewModel(availableTopics)
                {
                    Group = group,
                    GroupId = id
                };

                ViewBag.TopicList = model.TopicsList;
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /Group/ChooseTopicForGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseTopicForGroup(ChooseTopicForGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się dodać tematu do grupy.");
                return View(model);
            }

            _groupRepository.AddTopicToGroup(model.GroupId, model.SelectedTopic);
            return RedirectToAction(nameof(Details), new { id = model.GroupId });
        }

        #endregion

        #region Deadlines

        // GET: /Group/AddDeadline
        public async Task<IActionResult> AddDeadline(int id)
        {
            var user = await GetCurrentUserAsync();
            var group = _groupRepository.GetGroupById(id);

            if (user == null || group.Topic.TeacherId != user.Id)
            {
                return RedirectToAction(nameof(Index), "Topic", new { Message = TopicController.TopicMessageId.AddDeadlineError });
            }

            var model = new DeadlineViewModel
            {
                Group = @group,
                GroupId = @group.Id,
                TopicId = @group.Topic.Id
            };

            return View(model);
        }

        // POST: /Group/AddDeadline
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDeadline(DeadlineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var group = _groupRepository.GetGroupById(model.GroupId);
                model.Group = group;
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się dodać terminu cząstkowego.");
                return View(model);
            }

            var deadline = new Deadline
            {
                DeadlineDate = model.DeadlineDate.Value,
                Comment = model.Comment,
                GroupId = model.GroupId
            };

            _deadlineRepository.AddDeadline(deadline);
            return RedirectToAction(nameof(Details), "Topic", new { id = model.TopicId });
        }

        // GET: /Group/MarkDeadline
        public async Task<IActionResult> MarkDeadline(int id)
        {
            var user = await GetCurrentUserAsync();
            var deadline = _deadlineRepository.GetDeadlineById(id);

            if (user == null || deadline.Group.Topic.TeacherId != user.Id)
            {
                return RedirectToAction(nameof(Details), new { id = deadline.Group.TopicId });
            }

            var model = new MarkDeadlineViewModel
            {
                Id = deadline.Id,
                TopicId = (int)deadline.Group.TopicId,
                GroupId = deadline.GroupId,
                DeadlineDate = deadline.DeadlineDate,
                DateAndTime = deadline.GetDateAndTime,
                Mark = deadline.Mark.ToString(),
                Comment = deadline.Comment
            };

            return View(model);
        }

        // POST: /Group/MarkDeadline
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDeadline(MarkDeadlineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się ocenić terminu.");
                return View(model);
            }

            float mark;
            Single.TryParse(model.Mark, NumberStyles.Float, CultureInfo.InvariantCulture, out mark);

            var deadline = new Deadline
            {
                Id = model.Id,
                GroupId = model.GroupId,
                DeadlineDate = model.DeadlineDate,
                Mark = mark,
                Comment = model.Comment
            };

            _deadlineRepository.UpdateDeadline(deadline);

            return RedirectToAction(nameof(Details), new { id = model.TopicId });
        }

        #endregion

        #region Helpers

        public enum GroupMessageId
        {
            CreateGroupSuccess,
            Error,
            NoGroupToView,
            ErrorAddingCommentToGroup,
            LeaveGroupSuccess,
            DeleteGroupSuccess,
            NameChanged,
            NameChangeError
        }

        #endregion

        public IActionResult KickGroupFromTopic(int topicid, int groupid)
        {
            _groupRepository.RemoveTopicForGroup(groupid);
            return RedirectToAction(nameof(Details), "Topic", new { id = topicid });
        }
    }
}