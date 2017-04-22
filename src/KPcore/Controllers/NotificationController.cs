using System.Net;
using System.Threading.Tasks;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KPcore.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        public NotificationController(UserManager<ApplicationUser> userManager,
            INotificationRepository notificationRepository) : base(userManager, notificationRepository)
        { }

        [HttpPost]
        public JsonResult MaskAsSeen(int notificationId)
        {
            var userId = GetCurrentUserAsync().Result.Id;
            var marked = _notificationRepository.MarkUserNotificationAsSeen(userId, notificationId);
            if (!marked)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Couldn't find that notification.");
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Notification has been marked as seen.");
        }
    }
}