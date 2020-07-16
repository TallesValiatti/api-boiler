using Api.Core.Entities.Security;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infra.Repositories.Security
{
    public class UserRepository : GenericRepository<User>, IUserRepository<User> 
    {
        public UserRepository(AppDbContext context) : base(context)
        {}

        public Task<User> GetByEmail(string email)
        {
            var entity = _entities.FirstOrDefaultAsync(x => string.Compare(x.Email.Trim().ToLower(), email.Trim().ToLower()) == 0);
            return entity;
        }
    }
}
