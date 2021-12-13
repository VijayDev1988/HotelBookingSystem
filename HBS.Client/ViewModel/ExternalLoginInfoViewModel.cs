using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.ViewModel
{
    public class ExternalLoginInfoViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }

        //public ExternalLoginInfo ExternalLoginInfo { get; set; }
    }
}
