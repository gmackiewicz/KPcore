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
                message == TopicMessageId.CreateTopicSuccess ? "Nowy temat zosta� dodany pomy�lnie."
                : message == TopicMessageId.NoTopicToView ? "Brak tematu do wy�wietlenia."
                : message == TopicMessageId.Error ? "Wyst�pi� b��d."
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
                ModelState.AddModelError(string.Empty, "Nie uda�o si� utworzy� tematu.");
                return View(model);
            }

            var topic = new Topic
            {
                Title = model.Title,
                Description = model.Description,
                CreationDate = DateTime.Now,
                SubjectId = model.SelectedSubjectId,
                TeacherId = user.Id,
                MeetingsDate = model.MeetingsDate
            };

            _topicRepository.CreateTopic(topic);
            return RedirectToAction(nameof(Index), new { Message = TopicMessageId.CreateTopicSuccess });
        }


        // GET: /Topic/Details
        [HttpGet]
        public IActionResult Details(int? topicId)
        {
            if (topicId == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.NoTopicToView });
            }

            var topic = _topicRepository.GetTopicById(topicId);

            if (topic == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.NoTopicToView });
            }

            var model = new TopicDetailsViewModel
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                Teacher = topic.Teacher,
                Subject = topic.Subject,
                CreationDate = topic.CreationDate,
                MeetingsDate = topic.MeetingsDate
            };

            return View(model);
        }

        public IActionResult EditTopic(int id)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        public enum TopicMessageId
        {
            CreateTopicSuccess,
            Error,
            NoTopicToView
        }

        #endregion

    }
}