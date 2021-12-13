using HBS.Application.DTO.Account;
using HBS.Application.Enum;
using HBS.Application.Exceptions;
using HBS.Application.Interfaces;
using HBS.Application.Wrappers;
using HBS.Domain.Settings;
using HBS.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExternalLoginInfo = HBS.Application.DTO.Account.ExternalLoginInfo;

namespace HBS.Identity.Services
{
    public class AccountService : IAccountServices
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailService _emailService;l
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;


        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }
            if (!string.IsNullOrEmpty(request.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    throw new ApiException($"Invalid Credentials for '{request.Email}'.");
                }
            }
           
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            //var refreshToken = GenerateRefreshToken(ipAddress);
            //response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByEmailAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }

            IdentityResult result = null;
            if (string.IsNullOrEmpty(request.Password))
                result = await _userManager.CreateAsync(user, request.Password);
            else
                result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                return new Response<string>(user.Id, message: $"User Registered with EmailId {user.Email}");
            }

            throw new ApiException($"{result.Errors}");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }


        public async Task<Response<IdentityResult>> ExternalLoginSignInAsync(ExternalLoginInfo loginInfo)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider,
                   loginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return new Response<IdentityResult>("User signed Successfully");
            }
            else
            {
                var email = loginInfo.EmailId;
                var user = await _userManager.FindByEmailAsync(email);

                if (email != null)
                {
                    if (user == null)
                    {
                        var registerUser = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = loginInfo.UserName,
                            LastName = loginInfo.UserName
                        };

                        var userId = await _userManager.CreateAsync(registerUser);
                    }
                    var info = await _signInManager.GetExternalLoginInfoAsync();

                    var result = await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return new Response<IdentityResult>(result, "User Signed in successfully");

                }

                throw new ApiException("User signin failed, problem in external provider");
            }
        }

        public async Task<Response<object>> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
                return new Response<object>(user);

            throw new ApiException("User not found");
        }

        //public async Task<Response<IdentityResult>> AddLoginAndSignInAsync(IdentityUser user, ExternalLoginInfo info)
        //{
        //    var loginResult = await _userManager.AddLoginAsync(user, info);
        //    if (loginResult.Succeeded)
        //    {
        //        await _signInManager.SignInAsync(user, isPersistent: false);
        //        return new Response<IdentityResult>(loginResult);
        //    }

        //    throw new ApiException("Sign in failed");
        //}
    }
}
