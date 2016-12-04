using System;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
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
                : message == TopicMessageId.NoTopicToEdit ? "Brak tematu do edycji."
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
                if (model.Deadlines.Count() != 0)
                {
                    model.CurrentDeadline = _deadlineRepository.GetCurrentDeadline(@group.Id);
                }
            }

            return View(model);
        }

        // GET: /Topic/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUserAsync();
            var topic = _topicRepository.GetTopicById(id);

            if (user == null || topic == null || topic.TeacherId != user.Id)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.NoTopicToEdit });
            }

            var model = new EditTopicViewModel
            {
                TopicId = topic.Id,
                SubjectId = topic.SubjectId,
                CreationDate = topic.CreationDate,
                TeacherId = topic.TeacherId,
                Title = topic.Title,
                Description = topic.Description,
                MeetingsDate = topic.MeetingsDate
            };

            return View(model);
        }

        // POST: /Topic/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTopicViewModel model)
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

            var topic = new Topic
            {
                Id = model.TopicId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
                CreationDate = model.CreationDate,
                Title = model.Title,
                Description = model.Description,
                MeetingsDate = model.MeetingsDate
            };

            _topicRepository.EditTopic(topic);
            return RedirectToAction(nameof(Details), new { id = model.TopicId });
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

        #region TopicEntries

        // GET: /Topic/Entry
        public IActionResult Entry(int topicid, int? commentid)
        {
            var topic = _topicRepository.GetTopicById(topicid);
            if (topic == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.ErrorAddingCommentToTopic });
            }
            var model = new TopicCommentViewModel
            {
                Topic = topic,
                TopicId = topicid
            };

            var comment = _topicRepository.GetCommentById(commentid);
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

        // POST: /Topic/Entry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entry(TopicCommentViewModel model)
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

            var comment = new TopicEntry
            {
                TopicId = model.TopicId,
                AuthorId = user.Id,
                Content = model.Content,
            };

            if (model.CommentId == null)
            {
                comment.CreationDate = DateTime.Now;
                _topicRepository.AddComment(comment);
            }
            else
            {
                comment.Id = model.CommentId.Value;
                comment.CreationDate = model.CreationDate;
                comment.ModificationDate = DateTime.Now;
                _topicRepository.EditComment(comment);
            }

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

        #endregion

        #region Helpers

        public enum TopicMessageId
        {
            CreateTopicSuccess,
            Error,
            NoTopicToView,
            ErrorAddingCommentToTopic,
            TopicDeleted,
            AddDeadlineError,
            NoTopicToEdit
        }

        #endregion
    }
}