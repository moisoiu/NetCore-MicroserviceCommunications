using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Configuration
{
    public class Settings
    {
        public RefitUrls RefitUrls { get; set; }
        public AuthenticationServer AuthenticationServer { get; set; }
    }

    public class RefitUrls
    {
        public string UserApi { get; set; }
    }

    public class AuthenticationServer
    {
        public string ApiName { get; set; }
        public string Authority { get; set; }
        public bool RequiresHttps { get; set; }
        public string Secret { get; set; }
        public string GrantType { get; set; }
        public string Scope { get; set; }
    }
}
