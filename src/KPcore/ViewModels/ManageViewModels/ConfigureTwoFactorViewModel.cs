using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPcore.ViewModels.ManageViewModels
{
    public class ConfigureTwoFactorViewModel : BaseViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}
