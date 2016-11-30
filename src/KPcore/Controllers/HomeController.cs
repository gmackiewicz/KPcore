using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Models;
using KPcore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
        }

        public IActionResult Index()
        {
            return View(new BaseViewModel());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View(new BaseViewModel());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View(new BaseViewModel());
        }

        public IActionResult Error()
        {
            return View(new BaseViewModel());
        }

    }
}
