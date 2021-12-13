using HBS.Application.DTO.Account;
using HBS.Client.Services.Interfaces;
using HBS.Client.ViewModel;
using HBS.Identity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HBS.Client.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAccountServices _accountServices;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger,
                              IAccountServices accountServices,
                              SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _accountServices = accountServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountServices.RegisterAsync(model);
                ViewBag.ErrorTitle = result;
                ModelState.Clear();
                return View(new RegisterViewModel());
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            _logger.LogInformation("Login Page loaded");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await AuthenticateUser(loginViewModel.Email, loginViewModel.Password);
                if (response != null &&response.IsVerified)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("BookRoom", "Room");

                    return LocalRedirect(loginViewModel.ReturnUrl);
                }

                ModelState.AddModelError("", "User not found");
                loginViewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                return View(loginViewModel);
            }

            return View(loginViewModel);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "User", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Room/BookRoom");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider : {remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login info");
                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = null;
            user = await _userManager.FindByEmailAsync(email);

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                    info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                await AuthenticateUser(email);
                return LocalRedirect(returnUrl);
                //return RedirectToAction("BookRoom", "Room"); 
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                        return RedirectToAction("BookRoom", "Room");
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //get jwt
                    await AuthenticateUser(email);
                    return LocalRedirect(returnUrl);

                    //return RedirectToAction("BookRoom", "Room");
                }

                return View("Login", loginViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            ClearSession();
            return RedirectToAction("Login", "User");
        }
        private async Task<AuthenticationResponse> AuthenticateUser(string emailId, string password = null)
        {
            //get jwt
            var jwtRequest = new AuthenticationRequest
            {
                Email = emailId,
                Password = password
            };

            var jwtResponse = await _accountServices.AuthenticateUser(jwtRequest);
            if (jwtResponse == null) return null;
            HttpContext.Session.SetString("email", jwtResponse.Email);

            if (jwtResponse != null && jwtResponse.IsVerified)
            {
                if (password != null)
                {
                    var user = await _userManager.FindByEmailAsync(jwtResponse.Email);
                    await _signInManager.PasswordSignInAsync(user, jwtRequest.Password, false, false);
                }

                HttpContext.Session.SetString("token", jwtResponse.JWToken);
                return jwtResponse;
            }

            return null;
        }

        private void ClearSession()
        {
            HttpContext.Session.Clear();
        }
    }
}
