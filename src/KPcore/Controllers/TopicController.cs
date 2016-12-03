using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
using KPcore.ViewModels.GroupViewModels;
using KPcore.ViewModels.TopicViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    [Authorize]
    public class TopicController : BaseController
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ISubjectRepository _subjectRepository;

        public TopicController(UserManager<ApplicationUser> userManager,
            ITopicRepository topicRepository, ISubjectRepository subjectRepository) : base(userManager)
        {
            _topicRepository = topicRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<IActionResult> Index(TopicMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == TopicMessageId.CreateTopicSuccess ? "A new topic has been added."
                : message == TopicMessageId.Error ? "An error has occurred."
                : "";

            var user = await GetCurrentUserAsync();
            var model = new TeacherTopicsIndexViewModel
            {
                Topics = _topicRepository.GetAllUsersTopics(user.Id)
            };

            return View(model);
        }

        // GET: /Topic/Create
        public IActionResult Create()
        {
            var subjects = _subjectRepository.ListAll();

            var model = new CreateTopicViewModel(subjects);

            ViewBag.SubjectList = model.SubjectList;
            return View(model);
        }

        // POST: /Topic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTopicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to add new topic");
                return View(model);
            }

            var topic = new Topic
            {
                Title = model.Title,
                Description = model.Description,
                CreationDate = DateTime.Now,
                SubjectId = model.SelectedSubjectId,
                TeacherId = user.Id
            };

            _topicRepository.CreateTopic(topic);
            return RedirectToAction(nameof(Index), new { Message = TopicMessageId.CreateTopicSuccess });
        }

        #region Helpers

        public enum TopicMessageId
        {
            CreateTopicSuccess,
            Error,
        }

        #endregion
    }
}