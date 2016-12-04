using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IGroupRepository _groupRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IDeadlineRepository _deadlineRepository;

        public TopicController(UserManager<ApplicationUser> userManager,
            ITopicRepository topicRepository,
            IGroupRepository groupRepository,
            IDeadlineRepository deadlineRepository,
            ISubjectRepository subjectRepository) : base(userManager)
        {
            _topicRepository = topicRepository;
            _groupRepository = groupRepository;
            _subjectRepository = subjectRepository;
            _deadlineRepository = deadlineRepository;
        }

        public async Task<IActionResult> Index(TopicMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == TopicMessageId.CreateTopicSuccess ? "Nowy temat został dodany pomyślnie."
                : message == TopicMessageId.NoTopicToView ? "Brak tematu do wyświetlenia."
                : message == TopicMessageId.ErrorAddingCommentToTopic ? "Nie udało się dodać komentarza."
                : message == TopicMessageId.Error ? "Wystąpił błąd."
                : message == TopicMessageId.TopicDeleted ? "Temat został pomyślnie usunięty."
                : message == TopicMessageId.AddDeadlineError ? "Wystąpił błąd podczas dodawania terminu."
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
                ModelState.AddModelError(string.Empty, "Nie udało się utworzyć tematu.");
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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.NoTopicToView });
            }

            var topic = _topicRepository.GetTopicById(id);

            if (topic == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.NoTopicToView });
            }
            var group = _groupRepository.GetGroupByTopicId(id);
            var model = new TopicDetailsViewModel
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                Teacher = topic.Teacher,
                Subject = topic.Subject,
                CreationDate = topic.CreationDate,
                MeetingsDate = topic.MeetingsDate,
                TopicComments = _topicRepository.GetTopicComments(id)
            };

            if (group != null)
            {
                model.Group = @group;
                model.GroupLeader = _groupRepository.GetLeader(@group.Id);
                model.GroupMembers = _groupRepository.GetStudentsOfGroup(@group.Id);
                model.Deadlines = _deadlineRepository.GetDeadlinesByGroup(@group.Id);
                model.CurrentDeadline = _deadlineRepository.GetCurrentDeadline(@group.Id);
            }

            return View(model);
        }

        public IActionResult EditTopic(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> DeleteTopic(int id)
        {
            var user = await GetCurrentUserAsync();
            var topic = _topicRepository.GetTopicById(id);
            if (topic.TeacherId == user.Id)
            {
                _topicRepository.DeleteTopic(id);
            }
            return RedirectToAction(nameof(Index), new { Message = TopicMessageId.TopicDeleted });
        }

        #region Helpers

        public enum TopicMessageId
        {
            CreateTopicSuccess,
            Error,
            NoTopicToView,
            ErrorAddingCommentToTopic,
            TopicDeleted,
            AddDeadlineError
        }

        #endregion

        public IActionResult AddComment(int id)
        {
            var topic = _topicRepository.GetTopicById(id);

            if (topic == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.ErrorAddingCommentToTopic });
            }

            return View(new TopicCommentViewModel { Topic = topic, TopicId = topic.Id });
        }

        // POST: /Topic/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(TopicCommentViewModel model)
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

            var comment = new TopicEntry
            {
                TopicId = model.TopicId,
                AuthorId = user.Id,
                Content = model.Content,
                CreationDate = DateTime.Now
            };

            _topicRepository.AddComment(comment);
            return RedirectToAction(nameof(Details), new { id = comment.TopicId });
        }


        // GET: /Topic/EditComment
        public IActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.Error });
            }

            var comment = _topicRepository.GetCommentById(id);

            var model = new TopicCommentViewModel
            {
                CommentId = comment.Id,
                Topic = comment.Topic,
                TopicId = comment.TopicId,
                Content = comment.Content,
                CreationDate = comment.CreationDate
            };

            return View(model);
        }

        // POST: /Topic/EditComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(TopicCommentViewModel model)
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

            var comment = new TopicEntry
            {
                Id = model.CommentId.Value,
                TopicId = model.TopicId,
                AuthorId = user.Id,
                Content = model.Content,
                CreationDate = model.CreationDate,
                ModificationDate = DateTime.Now
            };

            _topicRepository.EditComment(comment);

            return RedirectToAction(nameof(Details), new { id = comment.TopicId });
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _topicRepository.GetCommentById(id);
            var user = await GetCurrentUserAsync();

            if (comment.AuthorId == user.Id)
            {
                _topicRepository.DeleteComment(id);
            }
            return RedirectToAction(nameof(Details), new { id = comment.TopicId });
        }

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

        // POST: /Topic/MarkDeadline
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
    }
}