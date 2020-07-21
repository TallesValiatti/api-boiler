using Api.Core.Entities.Security;
using Api.Core.Enums.Security;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infra.Repositories.Security
{
    public class SignInRegisterRepository : GenericRepository<SignInRegister>, ISignInRegisterRepository<SignInRegister> 
    {
        public SignInRegisterRepository(AppDbContext context) : base(context)
        {}

       
        public async Task<SignInRegister> GetLastSignInByUserAsync(Guid userId, EnumSignInType signIntType)
        {
                var entity = await _entities.Where(x => x.UserId == userId && x.SignInType == signIntType)
                                    .OrderBy(x => x.CreatedAt)
                                    .LastOrDefaultAsync();
                return entity;
            }
        }
}
