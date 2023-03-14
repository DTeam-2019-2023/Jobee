using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Jobee.Controllers
{
    public class AccountController : Controller
    {
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
            
        [HttpPost("/Account/Login")]
        public IActionResult LoginForm([Bind("username, password")] SigninModel signinModel)
        {
            if(ModelState.IsValid)
            return Content($"username: {signinModel.username}, password: {signinModel.password}");
            return View(nameof(Login));
        }

        public class SignupModel
        {
            [Required]
            public string username { get; set; }
            [Required]
            [DataType(dataType:DataType.Password)]
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
        public IActionResult SignupForm([Bind("username, password, rePassword, email")] SignupModel signupModel)
        {
            if (ModelState.IsValid)
                return Content($"username: {signupModel.username}, password: {signupModel.password}, email: {signupModel.email}");
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
                return Content($"email: {forgetPasswordModel.email}");
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
        public IActionResult CreateNewPassword()
        {
            return View();
        }
        [HttpPost("/Account/CreateNewPassword")]
        public IActionResult CreateNewPassword([Bind("newPassword, reNewPassword")] CreateNewPasswordModel createNewPasswordModel)
        {
            if (ModelState.IsValid)
                return Content($"CreateNewPassword: {createNewPasswordModel.newPassword}");
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
    }
}
