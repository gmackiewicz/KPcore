using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KPcore.Models;

namespace KPcore.ViewModels.GroupViewModels
{
    public class GroupDetailsViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int? TopicId { get; set; }

        public Topic Topic { get; set; }

        public string Name { get; set; }

        public IEnumerable<ApplicationUser> StudentsList { get; set; }

        public ApplicationUser GroupLeader { get; set; }
        public IEnumerable<GroupComment> GroupComments { get; set; }
    }
}
