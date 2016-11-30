using System.Linq;
using System.Threading.Tasks;
using KPcore.Data;
using KPcore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KPcore.Models;
using KPcore.Services;
using KPcore.ViewModels.AdminViewModels;

namespace KPcore.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly ISubjectRepository _subjectRepository;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ISubjectRepository subjectRepository) : base(userManager)
        {
            _subjectRepository = subjectRepository;
        }

        // GET: /Admin/Index
        [HttpGet]
        public async Task<IActionResult> Index(AdminMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == AdminMessageId.AddSubjectSuccess ? "A new subject has been added."
                : message == AdminMessageId.Error ? "An error has occurred."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null || user.Status != 2)
            {
                return View("Error");
            }
            var model = new AdminIndexViewModel
            {
                Subjects = _subjectRepository.ListAll()
            };
            return View(model);
        }
        
        // GET: /Admin/AddSubject
        public IActionResult AddSubject()
        {
            return View(new AddSubjectViewModel());
        }

        // POST: /Admin/AddSubject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null || user.Status != 2)
            {
                ModelState.AddModelError(string.Empty, "Failed to add new subject");
                return View(model);
            }
            var subject = new Subject
            {
                Name = model.Name,
                Description = model.Description
            };
            _subjectRepository.Add(subject);
            return RedirectToAction(nameof(Index), new { Message = AdminMessageId.AddSubjectSuccess });
        }
        
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum AdminMessageId
        {
            AddSubjectSuccess,
            Error,
        }
        
        #endregion
    }
}
