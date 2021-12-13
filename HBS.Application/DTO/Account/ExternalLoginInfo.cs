using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.DTO.Account
{
    public class ExternalLoginInfo
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
    }
}
