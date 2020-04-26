using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    internal static class ResourceManager
    {
        public static IEnumerable<ApiResource> Apis()
        {
            return new List<ApiResource>
            {   
                new ApiResource("app.api.backoffice.gateway","Backoffice.Gateway"),
                new ApiResource("app.api.medical.gateway","Medical.Gateway"),
                new ApiResource("app.api.weather","Weather Apis")
            };
        }
    }
}
