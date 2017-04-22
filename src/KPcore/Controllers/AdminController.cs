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
            INotificationRepository notificationRepository,
            ISubjectRepository subjectRepository) : base(userManager, notificationRepository)
        {
            _subjectRepository = subjectRepository;
        }

        // GET: /Admin/Index
        [HttpGet]
        public async Task<IActionResult> Index(AdminMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == AdminMessageId.AddSubjectSuccess ? "Nowy przedmiot został dodany pomyślnie."
                : message == AdminMessageId.Error ? "Wystąpił błąd."
                : message == AdminMessageId.RemoveSubjectSuccess ? "Przedmiot został usunięty pomyślnie."
                : message == AdminMessageId.RemoveSubjectFailed ? "Nie udało się usunąć przedmiotu: nie jesteś Adminem!"
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

        // GET: /Admin/Subject
        public async Task<IActionResult> Subject(int? id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null || user.Status != 2)
            {
                return RedirectToAction(nameof(Index), "Home", new { AdminMessageId.Error });
            }

            var subject = _subjectRepository.FindSubjectById(id);
            var model = new SubjectViewModel();

            ViewData["Title"] = "Dodaj przedmiot";
            if (subject != null)
            {
                model.SubjectId = subject.Id;
                model.Name = subject.Name;
                model.Description = subject.Description;
                ViewData["Title"] = "Edytuj przedmiot";
            }
            return View(model);
        }


        // POST: /Group/Subject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subject(SubjectViewModel model)
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

            var subject = new Subject
            {
                Id = 1,
                Name = model.Name,
                Description = model.Description,
            };

            if (model.SubjectId == null)
            {
                _subjectRepository.AddSubject(subject);
            }
            else
            {
                subject.Id = model.SubjectId.Value;
                _subjectRepository.EditSubject(subject);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveSubject(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null || user.Status != 2)
            {
                return RedirectToAction(nameof(Index), new { Message = AdminMessageId.RemoveSubjectFailed });
            }
            var subject = _subjectRepository.FindSubjectById(id);
            _subjectRepository.RemoveSubject(subject);
            return RedirectToAction(nameof(Index), new { Message = AdminMessageId.RemoveSubjectSuccess });
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
            RemoveSubjectSuccess,
            RemoveSubjectFailed
        }

        #endregion
    }
}
