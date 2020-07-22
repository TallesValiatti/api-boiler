using Api.Core.Entities;
using Api.Core.Entities.Security;
using Api.Core.Enums.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Interfaces.Infra.Repositories.Security
{
    public interface ISignInRegisterRepository<T> : IGenerericRepository<T> where T : BaseEntity
    {
        Task<SignInRegister> GetLastSignInByUserAsync(Guid userId, EnumSignInType signIntType);
    }
}
