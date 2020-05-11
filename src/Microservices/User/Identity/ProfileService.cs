using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.BusinessLayer;

namespace User.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly IUserLogic userLogic;

        public ProfileService(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userAccount = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var user = await userLogic.GetUser<Entities.User>(x => x.Account == userAccount);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString())
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userAccount = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var user = await userLogic.GetUser<Entities.User>(x => x.Account == userAccount);

            context.IsActive = (user != null) && user.IsActive;
        }
    }
}
