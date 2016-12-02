using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
using KPcore.ViewModels.GroupViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(UserManager<ApplicationUser> userManager,
            IGroupRepository groupRepository) : base(userManager)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GroupMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == GroupMessageId.CreateGroupSuccess ? "Nowa grupa zosta³a utworzona."
                : message == GroupMessageId.NoGroupToView ? "Nie ma takiej grupy."
                : message == GroupMessageId.ErrorAddingCommentToGroup ? "B³¹d podczas dodawania nowego komentarza."
                : message == GroupMessageId.Error ? "Wyst¹pi³ b³¹d."
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
                ModelState.AddModelError(string.Empty, "Nie uda³o siê stworzyæ nowej grupy.");
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

            return View(new NewGroupCommentViewModel { Group = group, GroupId = group.Id });

        }

        // POST: /Group/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(NewGroupCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê dodaæ komentarza.");
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
    }
}