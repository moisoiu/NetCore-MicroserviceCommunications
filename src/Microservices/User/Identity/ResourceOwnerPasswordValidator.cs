using DTO.User;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.BusinessLayer;

namespace User.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserLogic userLogic;

        public ResourceOwnerPasswordValidator(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await userLogic.GetUser<Entities.User>(x => x.Account == context.UserName.Trim());

            if (!await userLogic.VerifyUserLogin(context.UserName.Trim(), context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            context.Result = new GrantValidationResult(context.UserName, OidcConstants.AuthenticationMethods.Password);            
        }
    }
}
