using System;
using System.Collections.Generic;
using System.Linq;
using KPcore.Data;
using KPcore.Interfaces;
using KPcore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KPcore.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        IEnumerable<ApplicationUser> IUserRepository.ListAll()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public ApplicationUser GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
