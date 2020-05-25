using DTO.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, object> RefreshTokenPayload(this Dictionary<string, object> dictionary, string refreshToken, Settings settings)
        {
            dictionary = new Dictionary<string, object>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", settings.AuthenticationServer.ApiName },
                { "client_secret", settings.AuthenticationServer.Secret }
            };

            return dictionary;
        }

        public static Dictionary<string, object> RevokeRefreshTokenPayload(this Dictionary<string, object> dictionary, string refreshToken, Settings settings)
        {
            dictionary = new Dictionary<string, object>
            {
                { "token", refreshToken },
                { "token_type_hint", "refresh_token" },
                { "client_id", settings.AuthenticationServer.ApiName },
                { "client_secret", settings.AuthenticationServer.Secret }
            };

            return dictionary;
        }

        public static void AuthenticationPayload(this Dictionary<string, object> dictionary, dynamic request, Settings settings)
        {
            dictionary.Add("grant_type", settings.AuthenticationServer.GrantType);
            dictionary.Add("username", request.Account);
            dictionary.Add("password", request.Password);
            dictionary.Add("scope", string.Join(" ", settings.AuthenticationServer.Scope, "offline_access"));
            dictionary.Add("client_id", settings.AuthenticationServer.ApiName);
            dictionary.Add("client_secret", settings.AuthenticationServer.Secret);
        }
    }
}