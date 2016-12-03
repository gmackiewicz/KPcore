﻿using System;
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
                message == TopicMessageId.CreateTopicSuccess ? "Nowy temat został dodany pomyślnie."
                : message == TopicMessageId.NoTopicToView ? "Brak tematu do wyświetlenia."
                : message == TopicMessageId.ErrorAddingCommentToTopic ? "Nie udało się dodać komentarza."
                : message == TopicMessageId.Error ? "Wystąpił błąd."
                : message == TopicMessageId.TopicDeleted ? "Temat został pomyślnie usunięty."
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
                MeetingsDate = topic.MeetingsDate,
                TopicComments = _topicRepository.GetTopicComments(topicId)
            };

            return View(model);
        }

        public IActionResult EditTopic(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> DeleteTopic(int topicid)
        {
            var user = await GetCurrentUserAsync();
            var topic = _topicRepository.GetTopicById(topicid);
            if (topic.TeacherId == user.Id)
            {
                _topicRepository.DeleteTopic(topicid);
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
            TopicDeleted
        }

        #endregion

        public IActionResult AddComment(int topicId)
        {
            var topic = _topicRepository.GetTopicById(topicId);

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
            return RedirectToAction(nameof(Details), new { topicId = comment.TopicId });
        }


        // GET: /Topic/EditComment
        public IActionResult EditComment(int? commentId)
        {
            if (commentId == null)
            {
                return RedirectToAction(nameof(Index), new { Message = TopicMessageId.Error });
            }

            var comment = _topicRepository.GetCommentById(commentId);

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

            return RedirectToAction(nameof(Details), new { topicId = comment.TopicId });
        }

        public async Task<IActionResult> DeleteComment(int commentid)
        {
            var comment = _topicRepository.GetCommentById(commentid);
            var user = await GetCurrentUserAsync();

            if (comment.AuthorId == user.Id)
            {
                _topicRepository.DeleteComment(commentid);
            }
            return RedirectToAction(nameof(Details), new { topicId = comment.TopicId });
        }
    }
}