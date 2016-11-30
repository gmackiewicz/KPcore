using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace KPcore.ViewModels.ManageViewModels
{
    public class IndexViewModel : BaseViewModel
    {
        public int UserStatus { get; set; }

        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}
