using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPcore.ViewModels.GroupViewModels
{
    public class AddMemberToGroupViewModel : BaseViewModel
    {
        public int GroupId { get; set; }

        public List<SelectListItem> UsersList { get; private set; }

        [Required]
        [Display(Name = "Użytkownik")]
        public string SelectedUser { get; set; }

        public AddMemberToGroupViewModel() { }

        public AddMemberToGroupViewModel(List<ApplicationUser> membersToAdd)
        {
            UsersList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "wybierz użytkownika",
                    Value = ""
                }
            };

            foreach (var s in membersToAdd)
            {
                UsersList.Add(new SelectListItem
                {
                    Text = "[" + s.IndexNumber + "] " + s.FirstName + " " + s.LastName,
                    Value = s.Id
                });
            }
        }
    }
}