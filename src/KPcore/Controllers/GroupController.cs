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

        public async Task<IActionResult> Index()
        {
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
                ModelState.AddModelError(string.Empty, "Failed to add new group");
                return View(model);
            }

            var group = new Group
            {
                Name = model.Name
            };

            _groupRepository.CreateGroup(group, user.Id);
            return RedirectToAction(nameof(Index), new { Message = GroupMessageId.CreateGroupSuccess });
        }

        #region Helpers

        public enum GroupMessageId
        {
            CreateGroupSuccess,
            Error,
        }

        #endregion
    }
}