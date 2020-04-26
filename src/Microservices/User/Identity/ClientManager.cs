using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    internal static class ClientManager
    {
        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
            {
                    new Client
                    {
                         ClientName = "Backoffice.Gateway",
                         ClientId = "app.api.backoffice.gateway",
                         AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                         ClientSecrets = { new Secret("eb300de4-add9-42f4-a3ac-abd3c60f1919".Sha256()) },
                         AllowedScopes = { "app.api.backoffice.gateway", IdentityServerConstants.StandardScopes.OpenId },
                         AllowOfflineAccess = true
                    },
                    //new Client
                    //{
                    //     ClientName = "Medical.Gateway",
                    //     ClientId = "Medical.Gateway",
                    //     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //     ClientSecrets = { new Secret("9c400fdc-224f-42d3-b78f-02d6748cf473".Sha256()) },
                    //     AllowedScopes = { "app.api.medical.gateway" }
                    //},
                    //new Client
                    //{
                    //     ClientName = "Client Application2",
                    //     ClientId = "3X=nNv?Sgu$S",
                    //     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //     ClientSecrets = { new Secret("1554db43-3015-47a8-a748-55bd76b6af48".Sha256()) },
                    //     AllowedScopes = { "app.api.weather" }
                    //}
            };
        }
    }
}
