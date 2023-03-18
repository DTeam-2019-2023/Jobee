using Jobee_API.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text.Json;
using static Jobee.Controllers.AccountController;
using static System.Net.WebRequestMethods;

namespace Jobee.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client = null;
        private string SignupUserAPIUrl = null;
        private string LoginUserAPIUrl = null;


        public AccountController()
        {
        }

        public IActionResult Index()
        {
            return NotFound();
        }
        public IActionResult Login()
        {
            return View();
        }

        public class SigninModel
        {
            [Required]
            public string username { get; set; }
            [Required]
            public string password { get; set; }
        }
        //[BindProperty]
        //public SigninModel signinModel { get; set; } = default!;

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginForm([Bind("username, password")] SigninModel signinModel)
        {
            if (ModelState.IsValid)
            {
                var (token, type) = await Fetcher.LoginAsync(model: signinModel, loginUri: "https://localhost:7063/api/Users/login");

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError("", "Username or pass word wrong");
                    return View(nameof(Login));
                }
                //var respone = await client.PostAsJsonAsync(requestUri: LoginUserAPIUrl, signinModel);
                //string strData = await respone.Content.ReadAsStringAsync();
                //dynamic token = JObject.Parse(strData);
                //var options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //};

                //if (respone == null)
                //{
                //    return View(nameof(Login));
                //}
                Response.Cookies.Append("jwt", token);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, signinModel.username),
            new Claim(ClaimTypes.Role, type),
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction(nameof(Index), nameof(User));
            }
            return View(nameof(Login));
        }

        public class SignupModel
        {
            [Required]
            public string username { get; set; }
            [Required]
            [DataType(dataType: DataType.Password)]
            public string password { get; set; }
            [Required]
            [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
            public string rePassword { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "The Email field is not a valid e-mail address.")]
            public string email { get; set; }

        }
        //[BindProperty]
        //public SignupModel signupModel { get; set; } = default!;
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost("/Account/Signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignupForm([Bind("username, password, rePassword, email")] SignupModel signupModel)
        {
            if (ModelState.IsValid)
            {
                if (await Fetcher.SignupAsync(signupModel, "https://localhost:7063/api/Users/signup"))
                    return View(nameof(Login));
            }
            return View(nameof(Signup));

        }
        public class ForgetPasswordModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "The Email field is not a valid e-mail address.")]
            public string email { get; set; }
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost("/Account/ForgetPassword")]
        public IActionResult ForgetPassword([Bind("email")] ForgetPasswordModel forgetPasswordModel)
        {
            if (ModelState.IsValid)
                Fetcher.Custom(async client =>
                {
                    var res = await client.GetAsync($"https://localhost:7063/api/ForgotPwd/checkMail/{forgetPasswordModel.email}");
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string strData = await res.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(strData)) return;

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var value = JsonSerializer.Deserialize<dynamic>(strData, options);
                    }
                });
            return View(nameof(ForgetPassword));

        }
        public class ChangePasswordModel
        {

            [Required]
            [DataType(dataType: DataType.Password)]
            public string oldPassword { get; set; }
            [Required]
            [DataType(dataType: DataType.Password)]
            public string newPassword { get; set; }
            [Required]
            [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string reNewPassword { get; set; }
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost("/Account/ChangePassword")]
        public IActionResult ChangePassword([Bind("oldPassword, newPassword, reNewPassword")] ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
                return Content($"changePasswordModel: {changePasswordModel.oldPassword}, newPassword: {changePasswordModel.newPassword}");
            return View(nameof(ChangePassword));

        }
        public class CreateNewPasswordModel
        {
            [Required]
            [DataType(dataType: DataType.Password)]
            public string newPassword { get; set; }
            [Required]
            [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string reNewPassword { get; set; }
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewPassword(string email, string key)
        {
            bool valid = false;
            await Fetcher.Custom(async fetcher =>
            {
                var res = await fetcher.GetAsync($"https://localhost:7063/api/ForgotPwd/isKeyExist?key={key}");
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                    valid = string.IsNullOrEmpty(strData);
                }
            });

            if (valid)
            {
                return View();
            }
            return NotFound();
        }
        [HttpPost("/Account/CreateNewPassword")]
        public async Task<IActionResult> CreateNewPasswordAsync(string key, [Bind("newPassword, reNewPassword")] CreateNewPasswordModel createNewPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var new_pwd = createNewPasswordModel.newPassword;
                await Fetcher.Custom(async fetcher =>
                {

                    var res = await fetcher.PostAsJsonAsync($"https://localhost:7063/api/ForgotPwd/recover", new JObject(key, new_pwd));
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string strData = await res.Content.ReadAsStringAsync();
                    }
                });
                return Content($"CreateNewPassword: {createNewPasswordModel.newPassword}");
            }
            return View(nameof(CreateNewPassword));
        }
        public class EntryNewEmailModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "The Email field is not a valid e-mail address.")]
            public string email { get; set; }
        }
        public IActionResult EntryNewEmail()
        {
            return View();
        }
        [HttpPost("/Account/EntryNewEmail")]
        public IActionResult EntryNewEmail([Bind("email")] EntryNewEmailModel entryNewEmailModel)
        {
            if (ModelState.IsValid)
                return Content($"EntryNewEmail: {entryNewEmailModel.email}");
            return View(nameof(EntryNewEmail));
        }

        public class VerifyConfirmModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "The Email field is not a valid e-mail address.")]
            public string email { get; set; }
        }
        public IActionResult VerifyConfirm()
        {
            return View();
        }
        [HttpPost("/Account/VerifyConfirm")]
        public IActionResult VerifyConfirm([Bind("email")] VerifyConfirmModel entryNewEmailModel)
        {
            if (ModelState.IsValid)
                return Content($"EntryNewEmail: {entryNewEmailModel.email}");
            return View(nameof(EntryNewEmail));
        }

        //logout
        [HttpPost]
        [Authorize(Roles = "emp,ad")]
        public async Task<IActionResult> Logout()
        {
            Fetcher fetcher = new Fetcher(new Fetcher.ConfigFetcher()
            {
                context = HttpContext,
                root = "https://localhost:7063/api"
            });

            if (await fetcher.LogoutAsync())
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
