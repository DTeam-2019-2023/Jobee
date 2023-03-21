using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text.Json;
using static Jobee.Controllers.AccountController;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        //[BindProperty]
        //public SigninModel signinModel { get; set; } = default!;

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginForm([Bind("username, password")] Jobee_API.Models.User model)
        {
            if (ModelState.IsValid)
            {
                var (token, type) = await Fetcher.LoginAsync(model: model, loginUri: "https://localhost:7063/api/Users/login");

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError(nameof(model.username), "Username or Password wrong");
                    return View(nameof(Login));
                }

                Response.Cookies.Append("jwt", token);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.username),
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
            [RegularExpression("^[a-zA-Z0-9_-]{3,15}$", ErrorMessage = "Please enter username between 3 and 15 characters with no special character")]
            public string username { get; set; }
            [Required]
            [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "The password must contain at least 8 characters and have at least 1 digit and 1 letter")]
            [DataType(dataType: DataType.Password)]
            public string password { get; set; }
            [Required]
            [Compare(nameof(this.password), ErrorMessage = "The password and confirmation password do not match.")]
            [DisplayName("Confirm Password")]
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
        public IActionResult ChangePassword([Bind("oldPassword, newPassword, reNewPassword")] ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                Fetcher fetcher = new Fetcher(new Fetcher.ConfigFetcher()
                {
                    context = HttpContext,
                    root = "https://localhost:7063/api"
                });
               
                    User result;
                    User value = new User
                    {
                        username = User.Identity.Name,
                        password = model.newPassword
                    };
                    fetcher.Update(out result, value);
                    if (result != null)
                        return RedirectToAction(nameof(Login));
        
            }    
            return View(nameof(ChangePassword));

        }

        //public class ChangeEmailModel
        //{
        //    [Required]
        //    [DataType(DataType.EmailAddress)]
        //    [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "The Email field is not a valid e-mail address.")]
        //    public string email { get; set; }
        //}

        //[HttpPost("/Account/ChangeEmail")]
        //public IActionResult ChangeEmail([Bind("email")] ChangeEmailModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Fetcher fetcher = new Fetcher(new Fetcher.ConfigFetcher()
        //        {
        //            context = HttpContext,
        //            root = "https://localhost:7063/api"
        //        });

        //        Profile result;
        //        Profile value = new Profile
        //        {
        //            Email = model.email
        //        };
        //        fetcher.Update(out result, value);
        //        if (result != null)
        //            return RedirectToAction(nameof(Login));

        //    }
        //    return View(nameof(ChangeEmail));

        //}

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
                var res = fetcher.GetAsync($"https://localhost:7063/api/ForgotPwd/isKeyExist?key={key}").Result;
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                    valid = !string.IsNullOrEmpty(strData);
                }
            });

            if (valid)
            {
                ViewData["key"] = key;
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
                    UriBuilder builder = new UriBuilder("https://localhost:7063/api/ForgotPwd/recover");
                    //builder.Query = $"key={key}&new_pwd={new_pwd}";
                    var res = await fetcher.PostAsJsonAsync(builder.Uri, new
                    {
                        key = key,
                        new_pwd = new_pwd
                    });
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
        public IActionResult EntryNewEmail([Bind("email")] EntryNewEmailModel model)
        {
            if (ModelState.IsValid)
            {
                Fetcher fetcher = new Fetcher(new Fetcher.ConfigFetcher()
                {
                    context = HttpContext,
                    root = "https://localhost:7063/api"
                });


                Fetcher.Custom(async client => {
                    var token = HttpContext.Request.Cookies["jwt"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var res = await client.PutAsJsonAsync(requestUri: "https://localhost:7063/api/User/ChangeEmail", new Profile { Email = model.email});
                    if (res.IsSuccessStatusCode)
                    {
                        
                    }
                });

                Profile result;
                Profile value = new Profile
                {
                    Email = model.email
                };
                fetcher.Update(out result, value);
                if (result != null)
                    return RedirectToAction(nameof(Login));

            }
            return View(nameof(EntryNewEmail));
        }


        //[HttpPost("/Account/EntryNewEmail")]
        //public IActionResult EntryNewEmail([Bind("email")] EntryNewEmailModel entryNewEmailModel)
        //{
        //    if (ModelState.IsValid)
        //        return Content($"EntryNewEmail: {entryNewEmailModel.email}");
        //    return View(nameof(EntryNewEmail));
        //}

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
