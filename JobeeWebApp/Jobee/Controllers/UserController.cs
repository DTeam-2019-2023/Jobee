using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using static Jobee.Controllers.AccountController;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static Jobee.Controllers.AdminController;

namespace Jobee.Controllers
{
    public interface IUserController
    {
        public Task<IActionResult> Index();
        public IActionResult AddEducation([Bind("Name, Major, StartDate, EndDate, GPA, Description")] Jobee_API.Models.model_Education model);
        public IActionResult AddProject([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] Jobee_API.Models.model_Project model);
        public IActionResult AddCertificate([Bind("Name, StartDate, EndDate, Url, Description")] Jobee_API.Models.model_Certificate model);
        public IActionResult AddActivity([Bind("Name, Role, StartDate, EndDate, Description")] Jobee_API.Models.model_Activity model);
        public IActionResult AddAward([Bind("Name, StartDate, EndDate, Description, Role")] Jobee_API.Models.model_Award model);

        public IActionResult ViewEducation(string id);
        public IActionResult ViewProject(string id);
        public IActionResult ViewCertificate(string id);
        public IActionResult ViewActivity(string id);
        public IActionResult ViewAward(string id);

        public IActionResult EditEducation(string id);
        public IActionResult EditProject(string id);
        public IActionResult EditCertificate(string id);
        public IActionResult EditActivity(string id);
        public IActionResult EditAward(string id);

        public IActionResult EditEducationForm(string id, [Bind("Name, Major, StartDate, EndDate, GPA, Description")] Jobee_API.Models.model_Education model);
        public IActionResult EditProjectForm(string id, [Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] Jobee_API.Models.model_Project model);
        public IActionResult EditCertificateForm(string id, [Bind("Name, StartDate, EndDate, Url, Description")] Jobee_API.Models.model_Certificate model);
        public IActionResult EditActivityForm(string id, [Bind("Name, Role, StartDate, EndDate, Description")] Jobee_API.Models.model_Activity model);
        public IActionResult EditAwardForm(string id, [Bind("Name, StartDate, EndDate, Description, Role")] Jobee_API.Models.model_Award model);
        //null
        //public IActionResult UpdateAvatar();
        public IActionResult CreateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model);

        public IActionResult UpdateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model);
        public IActionResult UpdateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model);
        public IActionResult CreateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model);

        public IActionResult DeleteContentNav(string navType, string id);

        public IActionResult SendRequest(string id);

    }
    [Authorize(Roles = "emp")]
    public class UserController : Controller, IUserController
    {
        private Fetcher fetcher;
        public UserController(IHttpContextAccessor context)
        {
            fetcher = new Fetcher(new Fetcher.ConfigFetcher()
            {
                context = context.HttpContext,
                root = "https://localhost:7063/api"
            });
        }
        public class UserCVModel
        {
            public Profile profile { get; set; } = default!;
            public CV general { get; set; } = new();

            public string avtUrl { get; set; } = default!;

            public List<Education> Educations { get; set; } = default!;
            public List<Project> Projects { get; set; } = default!;
            public List<Certificate> Certificates { get; set; } = default!;
            public List<Activity> Activitys { get; set; } = default!;
            public List<Award> Awards { get; set; } = default!;
            public _ModelManagerment modelPopup { get; set; } = new();
        }




        List<string> DesiredWorkLocations = new List<string>() { "An Giang", "Bà Rịa-Vũng Tàu", "Bạc Liêu", "Bắc Kạn", "Bắc Giang", "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An", "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa", "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái", "Phú Quốc", "Đà Nẵng", "Hải Phòng", "Hà Nội", "Thành phố Hồ Chí Minh", "Cần Thơ" };
        List<string> Degrees = new List<string>() { "High school diploma", "Associate's degree", "Bachelor's degree", "Master's degree", "Doctorate degree" };
        List<string> CurrentJobs = new List<string>() { "Collaborator", "Specialist - Staff", "Expert", "Team Leader - Supervisor", "Middle Manager", "Senior Manager" };
        List<string> WorkExperiences = new List<string>() { "No experience", "1 year", "2 years", "3 years", "4 years", "5 years", "Over 5 years" };
        List<string> WorkingForms = new List<string>() { "Full-time permanent", "Full-time temporary", "Part-time permanent", "Part-time temporary", "Consultancy contract", "Internship", "Other" };

        public UserCVModel _model { get; set; } = new();
        public string StatusMessage { get; set; }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");
            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            //var CvAPI = await client.GetAsync("https://localhost:7063/api/TbCvs/GetTbCvsById");


            TbProfile Tbprofile;
            fetcher.GetSingleAuto(out Tbprofile);
            if (Tbprofile != null)
            {
                _model.profile = new()
                {
                    FirstName = Tbprofile.FirstName,
                    LastName = Tbprofile.LastName,
                    DoB = Tbprofile.DoB,
                    Gender = Tbprofile.Gender,
                    PhoneNumber = Tbprofile.PhoneNumber,
                    Address = Tbprofile.Address,
                    SocialNetwork = Tbprofile.SocialNetwork,
                    Email = Tbprofile.Email,
                    DetailAddress = Tbprofile.DetailAddress,
                };
            }

            TbCv tbCV;
            fetcher.GetSingleAuto(out tbCV);
            if (tbCV != null)
            {
                _model.general = new()
                {
                    ApplyPosition = tbCV.ApplyPosition,
                    CurrentJob = tbCV.CurrentJob,
                    DesirySalary = tbCV.DesirySalary,
                    Degree = tbCV.Degree,
                    WorkExperience = tbCV.WorkExperience,
                    DesiredWorkLocation = tbCV.DesiredWorkLocation,
                    WorkingForm = tbCV.WorkingForm,
                    CarrerObjiect = tbCV.CarrerObject,
                    SoftSkill = tbCV.SoftSkill,
                    Avatar = "/images/Avatar/default_avt.jfif"
                };
                List<Education> edus;
                fetcher.GetAll(out edus);
                model_Education edu;
                if (edus != null)
                {
                    _model.Educations = edus;
                }

                List<Activity> acs;
                fetcher.GetAll(out acs);
                model_Activity ac;
                if (acs != null)
                {
                    _model.Activitys = acs;
                }

                List<Certificate> cers;
                fetcher.GetAll(out cers);
                model_Certificate cer;
                if (cers != null)
                {
                    _model.Certificates = cers;
                }

                List<Project> projs;
                fetcher.GetAll(out projs);
                model_Project proj;
                if (projs != null)
                {
                    _model.Projects = projs;
                }

                List<Award> aws;
                fetcher.GetAll(out aws);
                model_Award aw;
                if (aws != null)
                {
                    _model.Awards = aws;
                }
            }

            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, _model.general?.DesiredWorkLocation);
            ViewData["Degrees"] = getListItem("Degree", Degrees, _model.general?.Degree);
            ViewData["CurrentJobs"] = getListItem("Current Job", CurrentJobs, _model.general?.CurrentJob);
            ViewData["WorkExperiences"] = getListItem("Work Experience", WorkExperiences, _model.general?.WorkExperience);
            ViewData["WorkingForms"] = getListItem("Working Form", WorkingForms, _model.general?.WorkingForm);
            HttpContext.Session.SetString(nameof(_model), JsonConvert.SerializeObject(_model));
            return View(_model);
        }
        private List<SelectListItem> getListItem(string Title, List<string> data, string? selected)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(Title))
                result.Add(new SelectListItem { Value = "", Text = Title, Disabled = true });
            for (int i = 0; i < data.Count; i++)
            {
                result.Add(new SelectListItem { Value = data[i], Text = data[i], Selected = data[i].Equals(selected) });
            }
            return result;
        }
        private void serializedUserModelCV()
        {
            string serializedEmployeeFromSession = HttpContext.Session.GetString(nameof(_model))!;
            if (!string.IsNullOrEmpty(serializedEmployeeFromSession))
                _model = JsonConvert.DeserializeObject<UserCVModel>(serializedEmployeeFromSession)!;
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, _model.general?.DesiredWorkLocation);
            ViewData["Degrees"] = getListItem("Degree", Degrees, _model.general?.Degree);
            ViewData["CurrentJobs"] = getListItem("Current Job", CurrentJobs, _model.general?.CurrentJob);
            ViewData["WorkExperiences"] = getListItem("Work Experience", WorkExperiences, _model.general?.WorkExperience);
            ViewData["WorkingForms"] = getListItem("Working Form", WorkingForms, _model.general?.WorkingForm);
        }
        public IActionResult AddEducation([Bind("Name, Major, StartDate, EndDate, GPA, Description")] model_Education model)
        {
            if (ModelState.IsValid)
            {
                Education tbedu;
                var result = fetcher.Create(out tbedu, model);
                if (result)
                    return RedirectToAction(nameof(Index));
            }
            serializedUserModelCV();
            ModelState.AddModelError("addEducation", "invalid");
            return View(nameof(Index), _model);
        }

        public IActionResult AddActivity([Bind("Name, Role, StartDate, EndDate, Description")] model_Activity model)
        {
            if (ModelState.IsValid)
            {
                Activity tbac;
                var result = fetcher.Create(out tbac, model);
                if (result)
                    return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddProject([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            if (ModelState.IsValid)
            {
                Project tbproj;
                var result = fetcher.Create(out tbproj, model);
                if (result)
                    return RedirectToAction(nameof(Index));
                return Conflict();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddCertificate([Bind("Name, StartDate, EndDate, Url, Description")] model_Certificate model)
        {
            if (ModelState.IsValid)
            {
                Certificate tbcer;
                var result = fetcher.Create(out tbcer, model);
                if (result)
                    return RedirectToAction(nameof(Index));
                return Conflict();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddAward([Bind("Name, StartDate, EndDate, Description, Role")] model_Award model)
        {
            if (ModelState.IsValid)
            {
                Award tbaw;
                var result = fetcher.Create(out tbaw, model);
                if (result)
                    return RedirectToAction(nameof(Index));
            }
            serializedUserModelCV();
            ModelState.AddModelError("addAward", "invalid");
            return View(nameof(Index), _model);
        }

        [HttpPost, ActionName("EditEducation")]
        [ValidateAntiForgeryToken]
        public IActionResult EditEducationForm(string id, [Bind("Name, Major, StartDate, EndDate, GPA, Description")] model_Education model)
        {
            if (ModelState.IsValid)
            {
                Education data;
                fetcher.UpdateById(out data, model, id);
                return Ok(new { status = "success", data = model });
            }
            return PartialView("~/Views/User/Popup/Edit/_editEducation.cshtml", model);
        }

        [HttpPost, ActionName("EditActivity")]
        [ValidateAntiForgeryToken]
        public IActionResult EditActivityForm(string id, [Bind("Name, Role, StartDate, EndDate, Description")] model_Activity model)
        {
            if (ModelState.IsValid)
            {
                Activity data;
                fetcher.UpdateById(out data, model, id);
                return Ok(new { status = "success", data = model });
            }
            return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", model);
        }

        [HttpPost, ActionName("EditProject")]
        [ValidateAntiForgeryToken]

        public IActionResult EditProjectForm(string id, [Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            if (ModelState.IsValid)
            {
                Project data;
                fetcher.UpdateById(out data, model, id);
                return Ok(new { status = "success", data = model });
            }
            return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", model);
        }

        [HttpPost, ActionName("EditCertificate")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCertificateForm(string id, [Bind("Name, StartDate, EndDate, Url, Description")] model_Certificate model)
        {
            if (ModelState.IsValid)
            {
                Certificate data;
                fetcher.UpdateById(out data, model, id);
                return Ok(new { status = "success", data = model });
            }
            return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", model);
        }

        [HttpPost, ActionName("EditAward")]
        [ValidateAntiForgeryToken]

        public IActionResult EditAwardForm(string id, [Bind("Name, StartDate, EndDate, Description, Role")] model_Award model)
        {
            if (ModelState.IsValid)
            {
                Award data;
                fetcher.UpdateById(out data, model, id);
                return Ok(new { status = "success", data = model });
            }
            return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", model);
        }

        [HttpPost]
        public IActionResult ViewEducation(string id)
        {
            Education data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/View/_viewEducation.cshtml", new model_Education
                {
                    Name = data.Name,
                    Major = data.Major,
                    GPA = data.Gpa,
                    Description = data.Description!,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/View/_viewEducation.cshtml", new model_Education());

        }

        [HttpPost]
        public IActionResult ViewActivity(string id)
        {
            Activity data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/View/_viewActivity.cshtml", new model_Activity
                {
                    Name = data.Name,
                    Role = data.Role,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    Description = data.Description
                });
            }

            return PartialView("~/Views/User/Popup/View/_viewActivity.cshtml", new model_Activity());
        }

        [HttpPost]
        public IActionResult ViewProject(string id)
        {
            Project data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/View/_viewProject.cshtml", new model_Project
                {
                    Name = data.Name,
                    Description = data.Description,
                    Role = data.Role,
                    Technology = data.Technology,
                    TeamSize = data.TeamSize,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/View/_viewProject.cshtml", new model_Project());
        }

        [HttpPost]
        public IActionResult ViewCertificate(string id)
        {
            Certificate data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/View/_viewCertificate.cshtml", new model_Certificate
                {
                    Name = data.Name,
                    EndDate = data.EndDate,
                    StartDate = data.StartDate,
                    Url = data.Url,
                    Description = data.Description,
                    IsVertify = data.IsVertify
                });
            }
            return PartialView("~/Views/User/Popup/View/_viewCertificate.cshtml", new model_Certificate());

        }

        [HttpPost]
        public IActionResult ViewAward(string id)
        {
            Award data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/View/_viewAward.cshtml", new model_Award
                {
                    Name = data.Name,
                    Description = data.Description,
                    Role = data.Role,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/View/_viewAward.cshtml", new model_Award());
        }

        [HttpGet]
        public IActionResult EditEducation(string id)
        {
            Education data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/Edit/_editEducation.cshtml", new model_Education
                {
                    Name = data.Name,
                    Major = data.Major,
                    GPA = data.Gpa,
                    Description = data.Description!,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/Edit/_editEducation.cshtml", data);
        }

        [HttpGet]
        public IActionResult EditActivity(string id)
        {
            Activity data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", new model_Activity
                {
                    Name = data.Name,
                    Role = data.Role,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    Description = data.Description
                });
            }
            return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", data);
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            Project data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", new model_Project
                {
                    Name = data.Name,
                    Description = data.Description,
                    TeamSize = data.TeamSize,
                    Role = data.Role,
                    Technology = data.Technology,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", data);
        }

        [HttpGet]
        public IActionResult EditCertificate(string id)
        {
            Certificate data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", new model_Certificate
                {
                    Name = data.Name,
                    EndDate = data.EndDate,
                    StartDate = data.StartDate,
                    Url = data.Url,
                    Description = data.Description,
                    IsVertify = data.IsVertify
                });
            }
            return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", data);
        }

        [HttpGet]
        public IActionResult EditAward(string id)
        {
            Award data;
            fetcher.GetById(out data, id);
            if (data != null)
            {
                ViewData["id"] = data.Id;
                return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", new model_Award
                {
                    Name = data.Name,
                    Description = data.Description,
                    Role = data.Role,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate
                });
            }
            return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", data);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteContentNav(string navType, string id)
        {
            switch (navType)
            {
                case "Education":
                    fetcher.Remove<Education>(id);
                    break;
                case "Project":
                    fetcher.Remove<Project>(id);
                    break;
                case "Certificate":
                    fetcher.Remove<Certificate>(id);
                    break;
                case "Activity":
                    fetcher.Remove<Activity>(id);
                    break;
                case "Award":
                    fetcher.Remove<Award>(id);
                    break;
                default: return Conflict("not found type of activity");
            }
            return Ok(new { status = "success", id = id });

        }

        public IActionResult CreateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model)
        {
            TbCv cv;
            var result = fetcher.Create(out cv, model);
            if (result)
                return RedirectToAction(nameof(Index));
            return Conflict();
        }

        public IActionResult UpdateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model)
        {
            if (ModelState.IsValid)
            {
                TbCv cv;
                var result = fetcher.Update(out cv, model);
                if (result == (int)HttpStatusCode.OK)
                    return RedirectToAction(nameof(Index));
            }
            string serializedEmployeeFromSession = HttpContext.Session.GetString(nameof(_model))!;
            if (!string.IsNullOrEmpty(serializedEmployeeFromSession))
                _model = JsonConvert.DeserializeObject<UserCVModel>(serializedEmployeeFromSession)!;
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, _model.general?.DesiredWorkLocation);
            ViewData["Degrees"] = getListItem("Degree", Degrees, _model.general?.Degree);
            ViewData["CurrentJobs"] = getListItem("Current Job", CurrentJobs, _model.general?.CurrentJob);
            ViewData["WorkExperiences"] = getListItem("Work Experience", WorkExperiences, _model.general?.WorkExperience);
            ViewData["WorkingForms"] = getListItem("Working Form", WorkingForms, _model.general?.WorkingForm);
            return View(nameof(Index), _model);

        }

        public IActionResult CreateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model)
        {
            TbProfile profile;
            var result = fetcher.Create(out profile, model);
            if (result)
                return RedirectToAction(nameof(Index));
            return Conflict();
        }

        public IActionResult UpdateProfile([Bind(new[] { "LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email" })] Profile model)
        {
            if (ModelState.IsValid)
            {
                TbProfile profile;
                var result = fetcher.Update(out profile, model);
                if (result == (int)HttpStatusCode.OK)
                    return RedirectToAction(nameof(Index));
            }
            string serializedEmployeeFromSession = HttpContext.Session.GetString(nameof(_model))!;
            if (!string.IsNullOrEmpty(serializedEmployeeFromSession))
                _model = JsonConvert.DeserializeObject<UserCVModel>(serializedEmployeeFromSession)!;
            ModelState.AddModelError("UpdateProfile", "not valid");
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, _model.general?.DesiredWorkLocation);
            ViewData["Degrees"] = getListItem("Degree", Degrees, _model.general?.Degree);
            ViewData["CurrentJobs"] = getListItem("Current Job", CurrentJobs, _model.general?.CurrentJob);
            ViewData["WorkExperiences"] = getListItem("Work Experience", WorkExperiences, _model.general?.WorkExperience);
            ViewData["WorkingForms"] = getListItem("Working Form", WorkingForms, _model.general?.WorkingForm);
            return View(nameof(Index), _model);
        }

        public class UserChangePassword
        {
            [Required]
            [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "The password must contain at least 8 characters and have at least 1 digit and 1 letter")]
            [DataType(dataType: DataType.Password)]
            public string oddPassword { get; set; }

            [Required]
            [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "The password must contain at least 8 characters and have at least 1 digit and 1 letter")]
            [DataType(dataType: DataType.Password)]
            public string newPassword { get; set; }
            [Required]
            [Compare(nameof(this.rePassword), ErrorMessage = "The password and confirmation password do not match.")]
            [DisplayName("Confirm Password")]
            public string rePassword { get; set; }
        }



        [HttpPost, ActionName("SendRequest")]
        public IActionResult SendRequest(string id)
        {
            return View();
        }
    }

}
