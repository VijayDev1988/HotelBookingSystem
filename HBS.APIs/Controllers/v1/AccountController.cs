using HBS.Application.DTO.Account;
using HBS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalLoginInfo = HBS.Application.DTO.Account.ExternalLoginInfo;

namespace HBS.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountServices _accountService;

        public AccountController(IAccountServices accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("ExternalLoginSignIn")]
        public async Task<IActionResult> ExternalLoginSignInAsync(ExternalLoginInfo loginInfo)
        {
            return Ok(await _accountService.ExternalLoginSignInAsync(loginInfo));
        }

        [HttpGet("FindUser")]
        public async Task<IActionResult> FindByEmailAsync(string emailId)
        {
            return Ok(await _accountService.FindByEmailAsync(emailId));
        }

        [HttpGet("ExternalCallback")]
        public async Task<IActionResult> ExternalCallback(string returnUrl = null, string remoteError = null)
        {
            return Ok();
        }
    }
}
