using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Controllers;
using KPcore.Interfaces;
using KPcore.Models;

namespace KPcore.ViewModels
{
    public class BaseViewModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
