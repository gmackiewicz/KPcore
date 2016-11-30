using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPcore.ViewModels.AccountViewModels
{
    public class SendCodeViewModel : BaseViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
