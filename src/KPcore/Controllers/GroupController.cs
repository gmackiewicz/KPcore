using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Models;
using KPcore.ViewModels.StudentGroupsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    [Authorize]
    public class StudentGroupController : BaseController
    {
        public StudentGroupController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
        }
        
        public IActionResult Index()
        {
            return View(new StudentGroupIndexViewModel());
        }

        public IActionResult CreateNewGroup()
        {
            throw new NotImplementedException();
        }
    }
}