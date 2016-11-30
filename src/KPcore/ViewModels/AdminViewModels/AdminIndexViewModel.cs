﻿using System.Collections.Generic;
using KPcore.Models;
using Microsoft.AspNetCore.Identity;

namespace KPcore.ViewModels.AdminViewModels
{
    public class AdminIndexViewModel : BaseViewModel
    {
        public IEnumerable<Subject> Subjects { get; set; }
    }
}
