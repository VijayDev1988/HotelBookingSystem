using HBS.Application.DTO.Account;
using HBS.Client.ViewModel;
using HBS.Identity.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalLoginInfo = Microsoft.AspNetCore.Identity.ExternalLoginInfo;

namespace HBS.Client.Services.Interfaces
{
    public interface IAccountServices
    {
        Task<string> RegisterAsync(RegisterViewModel request);

        Task<IdentityResult> ExternalLoginSignInAsync(ExternalLoginInfo request);

        Task<ApplicationUser> FindByEmailAsync(string emailId);

        Task<AuthenticationResponse> AuthenticateUser(AuthenticationRequest request);

    }
}
