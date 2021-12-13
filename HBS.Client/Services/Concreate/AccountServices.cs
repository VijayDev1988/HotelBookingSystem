using AutoMapper;
using HBS.Application.DTO.Account;
using HBS.Application.Wrappers;
using HBS.Client.Services.Interfaces;
using HBS.Client.Utilities;
using HBS.Client.ViewModel;
using HBS.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExternalLoginInfo = Microsoft.AspNetCore.Identity.ExternalLoginInfo;

namespace HBS.Client.Services.Concreate
{
    public class AccountServices : IAccountServices
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public AccountServices(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }


        public async Task<string> RegisterAsync(RegisterViewModel request)
        {
            try
            {
                var registerUser = _mapper.Map<RegisterRequest>(request);

                var json = JsonConvert.SerializeObject(registerUser);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                var URI = $"{ApplicationsConstants.REGISTER_USER_API}";
                var output = await _httpClient.PostAsync(URI, stringContent);
                var user = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<string>>(user);

                if (result.Succeeded)
                {
                    return result.Message;
                }

                return result.Message;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IdentityResult> ExternalLoginSignInAsync(ExternalLoginInfo request)
        {
            try
            {
                var externalLoginInfo = new ExternalLoginInfoViewModel
                {
                    LoginProvider = request.LoginProvider,
                    ProviderDisplayName = request.ProviderDisplayName,
                    ProviderKey = request.ProviderKey,
                    EmailId = request.Principal.FindFirstValue(ClaimTypes.Email),
                    UserName = request.Principal.FindFirstValue(ClaimTypes.Name),
                    //ExternalLoginInfo = request
                };

                var json = JsonConvert.SerializeObject(externalLoginInfo);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var URI = $"{ApplicationsConstants.EXTERNAL_LOGIN_API}";
                var output = await _httpClient.PostAsync(URI, stringContent);

                var user = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<IdentityResult>>(user);
                if (result.Succeeded)
                {
                    return result.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string emailId)
        {
            try
            {
                var URI = $"{ApplicationsConstants.FIND_USER_API}{emailId}";
                var output = await _httpClient.GetAsync(URI);

                var user = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<ApplicationUser>>(user);
                if (result.Succeeded)
                {
                    return result.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthenticationResponse> AuthenticateUser(AuthenticationRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                
                var URI = $"{ApplicationsConstants.AUTHENTICATE_USER_API}";
                var output = await _httpClient.PostAsync(URI, stringContent);
                var user = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<AuthenticationResponse>>(user);
                if (result.Succeeded)
                {
                    return result.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}