using Api.Core.Entities;
using Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Interfaces.Infra.Repositories.Security
{
    public interface IUserRepository<T> : IGenerericRepository<T> where T : BaseEntity
    {
        Task<User> GetByEmail(string email);
    }
}
