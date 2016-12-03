using System;
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

        public GroupController(UserManager<ApplicationUser> userManager,
            IGroupRepository groupRepository,
            IUserRepository userRepository) : base(userManager)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GroupMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == GroupMessageId.CreateGroupSuccess ? "Nowa grupa została utworzona."
                : message == GroupMessageId.NoGroupToView ? "Nie ma takiej grupy."
                : message == GroupMessageId.ErrorAddingCommentToGroup ? "Błąd podczas dodawania nowego komentarza."
                : message == GroupMessageId.Error ? "Wystąpił błąd."
                : "";


            var user = await GetCurrentUserAsync();
            var model = new StudentGroupIndexViewModel
            {
                StudentGroups = _groupRepository.GetAllUsersGroup(user.Id)
            };

            return View(model);
        }

        // GET: /Group/Create
        public IActionResult Create()
        {
            return View(new CreateGroupViewModel());
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGroupViewModel model)
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
        public IActionResult Details(int? groupId)
        {
            if (groupId == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.NoGroupToView });
            }

            var group = _groupRepository.GetGroupById(groupId);

            if (group == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.NoGroupToView });
            }

            var studentsList = _groupRepository.GetStudentsOfGroup(groupId);

            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                TopicId = group.TopicId,
                Topic = group.Topic,
                StudentsList = studentsList,
                GroupLeader = _groupRepository.GetLeader(groupId),
                GroupComments = _groupRepository.GetGroupComments(groupId)
            };

            return View(model);
        }

        #region Helpers

        public enum GroupMessageId
        {
            CreateGroupSuccess,
            Error,
            NoGroupToView,
            ErrorAddingCommentToGroup
        }

        #endregion

        public IActionResult EditGroup()
        {
            throw new NotImplementedException();
        }

        // GET: /Group/AddComment
        public IActionResult AddComment(int? groupId)
        {
            if (groupId == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.ErrorAddingCommentToGroup });
            }

            var group = _groupRepository.GetGroupById(groupId);

            if (group == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.ErrorAddingCommentToGroup });
            }

            return View(new GroupCommentViewModel { Group = group, GroupId = group.Id });

        }

        // POST: /Group/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(GroupCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się dodać komentarza.");
                return View(model);
            }

            var comment = new GroupComment
            {
                GroupId = model.GroupId,
                AuthorId = user.Id,
                Content = model.Content,
                CreationDate = DateTime.Now
            };

            _groupRepository.AddComment(comment);
            return RedirectToAction(nameof(Details), new { groupId = comment.GroupId });
        }

        public async Task<IActionResult> RemoveMember(int groupid, string memberid)
        {
            var currentUser = await GetCurrentUserAsync();
            var groupLeader = _groupRepository.GetLeader(groupid);

            if (currentUser != null && currentUser.Id == groupLeader.Id)
            {
                _groupRepository.RemoveMemberFromGroup(groupid, memberid);
                return RedirectToAction(nameof(Details), new { groupId = groupid });
            }

            ModelState.AddModelError(string.Empty, "Nie udało się usunąć członka grupy.");
            return RedirectToAction(nameof(Details), new { groupId = groupid });
        }

        public async Task<IActionResult> AddMember(int groupid)
        {
            var currentUser = await GetCurrentUserAsync();
            var groupLeader = _groupRepository.GetLeader(groupid);

            if (currentUser == null || currentUser.Id != groupLeader.Id)
            {
                return RedirectToAction(nameof(Details), new { groupId = groupid });
            }

            var students = _userRepository.GetAllStudents();
            var groupMembers = _groupRepository.GetStudentsOfGroup(groupid).Select(gm => gm.Id);
            var membersToAdd = students.Where(s => (!groupMembers.Contains(s.Id))).ToList();

            var model = new AddMemberToGroupViewModel(membersToAdd);
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
            return RedirectToAction(nameof(Details), new { groupId = model.GroupId });
        }


        // GET: /Group/EditComment
        public IActionResult EditComment(int? commentId)
        {
            if (commentId == null)
            {
                return RedirectToAction(nameof(Index), new { Message = GroupMessageId.Error });
            }

            var comment = _groupRepository.GetCommentById(commentId);

            var model = new GroupCommentViewModel
            {
                CommentId = comment.Id,
                Group = comment.Group,
                GroupId = comment.GroupId,
                Content = comment.Content,
                CreationDate = comment.CreationDate
            };

            return View(model);
        }

        // POST: /Group/EditComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(GroupCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się edytować komentarza.");
                return View(model);
            }

            var comment = new GroupComment
            {
                Id = model.CommentId.Value,
                GroupId = model.GroupId,
                AuthorId = user.Id,
                Content = model.Content,
                CreationDate = model.CreationDate,
                ModificationDate = DateTime.Now
            };

            _groupRepository.EditComment(comment);

            return RedirectToAction(nameof(Details), new { groupId = comment.GroupId });
        }
    }
}