﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
using KPcore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager,
            INotificationRepository notificationRepository) : base(userManager, notificationRepository)
        {
        }

        public IActionResult Index()
        {
            return View(new BaseViewModel());
        }

        public IActionResult Error()
        {
            return View(new BaseViewModel());
        }

    }
}
