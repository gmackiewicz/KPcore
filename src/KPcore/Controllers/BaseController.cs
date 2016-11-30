using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Models;
using KPcore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KPcore.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var user = GetCurrentUserAsync();
            var controller = context.Controller as Controller;
            var model = controller?.ViewData.Model as BaseViewModel;
            if (model != null) model.CurrentUser = user.Result;

        }
        
        protected Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}