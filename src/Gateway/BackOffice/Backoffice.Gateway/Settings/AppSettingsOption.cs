using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway
{
    public class AppSettingsOption
    {
        public RefitUrls RefitUrls { get; set; }
    }

    public class RefitUrls
    {
        public string UserApi { get; set; }
        public string PatientApi { get; set; }
    }
}
