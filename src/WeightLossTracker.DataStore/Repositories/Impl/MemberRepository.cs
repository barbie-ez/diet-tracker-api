using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeightLossTracker.DataStore.Entitties;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class MemberRepository : UserManager<UserProfileModel>
    {
     
        public MemberRepository(IUserStore<UserProfileModel> store, IOptions<IdentityOptions> optionsAccessor, 
                            IPasswordHasher<UserProfileModel> passwordHasher, IEnumerable<IUserValidator<UserProfileModel>> userValidators, 
                            IEnumerable<IPasswordValidator<UserProfileModel>> passwordValidators, ILookupNormalizer keyNormalizer, 
                            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserProfileModel>> logger) 
                                : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        
        }

        public async Task<string> GetNameAsync(ClaimsPrincipal principal)
        {
            var user = await GetUserAsync(principal);
            return $"{user.FirstName} {user.LastName}";
        }

        
    }
}
