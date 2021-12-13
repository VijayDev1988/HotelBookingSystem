using HBS.Application.DTO.Account;
using HBS.Application.Wrappers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ExternalLoginInfo = HBS.Application.DTO.Account.ExternalLoginInfo;

namespace HBS.Application.Interfaces
{
    public interface IAccountServices
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task<Response<IdentityResult>> ExternalLoginSignInAsync(ExternalLoginInfo loginInfo);

        Task<Response<object>> FindByEmailAsync(string email);

        //Task<Response<IdentityResult>> AddLoginAndSignInAsync(IdentityUser user, ExternalLoginInfo info);
    }
}
