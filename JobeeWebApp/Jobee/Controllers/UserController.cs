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

namespace Jobee.Controllers
{
    public interface IUserController
    {

        public Task<IActionResult> Index();
        public IActionResult AddEducation([Bind("Name, Major, StartDate, EndDate, GPA, Description")] Jobee_API.Models.model_Education model);
        public IActionResult AddProject([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] Jobee_API.Models.model_Project model);
        public IActionResult AddCertificate([Bind("Name, StartDate, EndDate, Url")] Jobee_API.Models.model_Certificate model);
        public IActionResult AddActivity([Bind("Name, Role, StartDate, EndDate, Description")] Jobee_API.Models.model_Activity model);
        public IActionResult AddAward([Bind("Name, StartDate, EndDate, Description")] Jobee_API.Models.model_Award model);

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

        public IActionResult EditEducationForm([Bind("Name, Major, StartDate, EndDate, GPA, Description")] Jobee_API.Models.model_Education model);
        public IActionResult EditProjectForm([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] Jobee_API.Models.model_Project model);
        public IActionResult EditCertificateForm([Bind("Name, StartDate, EndDate, Url")] Jobee_API.Models.model_Certificate model);
        public IActionResult EditActivityForm([Bind("Name, Role, StartDate, EndDate, Description")] Jobee_API.Models.model_Activity model);
        public IActionResult EditAwardForm([Bind("Name, StartDate, EndDate, Description")] Jobee_API.Models.model_Award model);
        //null
        //public IActionResult UpdateAvatar();
        public IActionResult CreateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model);

        public IActionResult UpdateGeneral([Bind("ApplyPosition, CurrentJob, DesirySalary,  Degree, WorkExperience, DesiredWorkLocation, WorkingForm, CarrerObjiect, SoftSkill, Avatar")] CV model);
        public IActionResult UpdateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model);
        public IActionResult CreateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model);

        public IActionResult DeleteContentNav(string navType, string id);

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
            public CV general { get; set; } = default!;

            public string avtUrl { get; set; } = default!;

            public List<Education> Educations { get; set; } = default!;
            public List<Project> Projects { get; set; } = default!;
            public List<Certificate> Certificates { get; set; } = default!;
            public List<Activity> Activitys { get; set; } = default!;
            public List<Award> Awards { get; set; } = default!;
            public _ModelManagerment modelPopup { get; set; } = default!;
        }
        List<string> DesiredWorkLocations = new List<string>() { "An Giang", "Bà Rịa-Vũng Tàu", "Bạc Liêu", "Bắc Kạn", "Bắc Giang", "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An", "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa", "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái", "Phú Quốc", "Đà Nẵng", "Hải Phòng", "Hà Nội", "Thành phố Hồ Chí Minh", "Cần Thơ" };
        List<string> Degrees = new List<string>() { "High school diploma", "Associate's degree", "Bachelor's degree", "Master's degree", "Doctorate degree" };
        List<string> CurrentJobs = new List<string>() { "Collaborator", "Specialist - Staff", "Expert", "Team Leader - Supervisor", "Middle Manager", "Senior Manager" };
        List<string> WorkExperiences = new List<string>() { "No experience", "1 year", "2 years", "3 years", "4 years", "5 years", "Over 5 years" };
        List<string> WorkingForms = new List<string>() { "Full-time permanent", "Full-time temporary", "Part-time permanent", "Part-time temporary", "Consultancy contract", "Internship", "Other" };

        public UserCVModel _model { get; set; } = default!;
        public string StatusMessage { get; set; }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");
            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Accept.Add(contentType);
            //var CvAPI = await client.GetAsync("https://localhost:7063/api/TbCvs/GetTbCvsById");

            TbCv tbCV;
            fetcher.GetSingleAuto(out tbCV);
            CV cv = default!;
            if (tbCV.Id != null)
            {
                cv = new()
                {
                    ApplyPosition = tbCV.ApplyPosition,
                    CurrentJob = tbCV.CurrentJob,
                    DesirySalary = tbCV.DesirySalary,
                    Degree = tbCV.Degree,
                    WorkExperience= tbCV.WorkExperience,
                    DesiredWorkLocation= tbCV.DesiredWorkLocation,
                    WorkingForm= tbCV.WorkingForm,
                    CarrerObjiect=tbCV.CarrerObject,
                    SoftSkill= tbCV.SoftSkill,
                    Avatar= "/images/Members/1w2ta6TdDwqezXFEeQYVJw.jpg"
                };
            }

            TbProfile Tbprofile;
            fetcher.GetSingleAuto(out Tbprofile);
            Profile profile = default!;
            if (Tbprofile.Id != null)
            {
                profile = new() { 
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

            //List<Education> edus;
            //fetcher.GetAll(out edus);
            //model_Education edu;

            //if (edus.Count > 0)
            //{
            //    foreach (var item in edus)
            //    {
            //        edu = new()
            //        {
            //            Name = item.Name,
            //            Major = item.Major,
            //            StartDate = item.StartDate,
            //            EndDate = item.EndDate,
            //            GPA = item.Gpa,
            //            Description = item.Description
            //        };
            //    }
            //}

            _model = new()
            {
                Educations = new()
                {
                    new()
                    {
                        Name = "Toan",
                        Gpa = 4.0,
                        Id = "edu1"
                    },
                    new()
                    {
                        Name = "Van",
                        Gpa = 4.0,
                        Id = "edu2"
                    }
                },
                profile = profile,
                Activitys = new() {
                    new(){
                    Id = "Ac1",
                    Name = "Ac1",
                    EndDate = DateTime.Parse("1/1/2001"),
                    StartDate = DateTime.Parse("1/1/2001"),
                    Role = "Role1",
                    Description = "Description1"
                    },
                    new(){
                    Id = "Ac2",
                    Name = "Ac2",
                    EndDate = DateTime.Parse("1/1/2001"),
                    StartDate = DateTime.Parse("1/1/2001"),
                    Role = "Role2",
                    Description = "Description2"
                    }
                },
                Projects = new()
                {
                    new()
                    {
                        Id = "Pro1",
                        Name = "Pro1",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Role = "Role1",
                        Description = "Description1",
                        TeamSize = 1,
                        Technology = "Tech1"
                    },
                    new()
                    {
                        Id = "Pro2",
                        Name = "Pro2",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Role = "Role2",
                        Description = "Description2",
                        TeamSize = 2,
                        Technology = "Tech2"
                    }
                },
                Certificates = new()
                {
                    new()
                    {
                        Id = "Cer1",
                        Name = "Cer1",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Url = "Url1"
                    },
                    new()
                    {
                        Id = "Cer2",
                        Name = "Cer2",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Url = "Url2"
                    }
                },
                Awards = new()
                {
                    new()
                    {
                        Id = "Awa1",
                        Name = "Awa1",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Description = "Description1"
                    },
                    new()
                    {
                        Id = "Awa2",
                        Name = "Awa2",
                        EndDate = DateTime.Parse("1/1/2001"),
                        StartDate = DateTime.Parse("1/1/2001"),
                        Description = "Description2"
                    }
                },
                general = cv,
                modelPopup = new()
            };
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, _model.general?.DesiredWorkLocation);
            ViewData["Degrees"] = getListItem("Degree", Degrees, _model.general?.Degree);
            ViewData["CurrentJobs"] = getListItem("Current Job", CurrentJobs, _model.general?.CurrentJob);
            ViewData["WorkExperiences"] = getListItem("Work Experience", WorkExperiences, _model.general?.WorkExperience);
            ViewData["WorkingForms"] = getListItem("Working Form", WorkingForms, _model.general?.WorkingForm);
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

        public IActionResult AddEducation([Bind("Name, Major, StartDate, EndDate, GPA, Description")] model_Education model)
        {
            if (ModelState.IsValid)
            {
                Education tbedu;
                var result = fetcher.Create(out tbedu, model);
                if (result)
                    return RedirectToAction(nameof(Index));
                return Conflict();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("EditEducation")]
        [ValidateAntiForgeryToken]
        public IActionResult EditEducationForm([Bind("Name, Major, StartDate, EndDate, GPA, Description")] model_Education model)
        {
            if (ModelState.IsValid)
            {
                return Ok("success");
            }
            return PartialView("~/Views/User/Popup/Edit/_editEducation.cshtml", model);
        }

        public IActionResult AddProject([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            if (ModelState.IsValid)
            {
                return Content("OK");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddCertificate([Bind("Name, StartDate, EndDate, Url")] model_Certificate model)
        {
            if (ModelState.IsValid)
            {
                return Content("OK");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddActivity([Bind("Name, Role, StartDate, EndDate, Description")] model_Activity model)
        {
            if (ModelState.IsValid)
            {
                return Content("OK");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddAward([Bind("Name, StartDate, EndDate, Description")] model_Award model)
        {
            if (ModelState.IsValid)
            {
                return Content("OK");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ViewEducation(string id)
        {
            model_Education edu1 = new()
            {
                Name = "Toan",
                Major = "Toan",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                GPA = 4.0,
                Description = "Toan"
            };
            model_Education edu2 = new()
            {
                Name = "Van",
                Major = "Van",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                GPA = 7.0,
                Description = "Van"
            };
            if (id == "edu1")
            {
                return PartialView("~/Views/User/Popup/View/_viewEducation.cshtml", edu1);
            }
            return PartialView("~/Views/User/Popup/View/_viewEducation.cshtml", edu2);
        }

        public IActionResult ViewProject(string id)
        {
            model_Project pro1 = new()
            {
                Name = "Pro1",
                TeamSize = 1,
                Role = "Role1",
                Technology = "Tech1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/View/_viewProject.cshtml", pro1);
        }

        public IActionResult ViewCertificate(string id)
        {
            model_Certificate cer1 = new()
            {
                Name = "Cer1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Url = "Url1"
            };
            return PartialView("~/Views/User/Popup/View/_viewCertificate.cshtml", cer1);

        }

        public IActionResult ViewActivity(string id)
        {
            model_Activity act1 = new()
            {
                Name = "Act1",
                Role = "Role1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/View/_viewActivity.cshtml", act1);
        }

        public IActionResult ViewAward(string id)
        {
            model_Award awa1 = new()
            {
                Name = "Awa1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/View/_viewAward.cshtml", awa1);
        }
        [HttpGet]
        public IActionResult EditEducation(string id)
        {
            model_Education data = new()
            {
                Name = "Toan",
                Major = "Toan",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                GPA = 4.0,
                Description = "Toan"
            };
            return PartialView("~/Views/User/Popup/Edit/_editEducation.cshtml", data);
        }

        public IActionResult EditProject(string id)
        {
            model_Project model_Project = new()
            {
                Name = "Pro1",
                TeamSize = 1,
                Role = "Role1",
                Technology = "Tech1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", model_Project);

        }

        public IActionResult EditCertificate(string id)
        {
            model_Certificate cer1 = new()
            {
                Name = "Cer1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Url = "Url1"
            };
            return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", cer1);
        }

        public IActionResult EditActivity(string id)
        {
            model_Activity act1 = new()
            {
                Name = "Act1",
                Role = "Role1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", act1);
        }

        public IActionResult EditAward(string id)
        {
            model_Award awa1 = new()
            {
                Name = "Awa1",
                StartDate = DateTime.Parse("1/1/2001"),
                EndDate = DateTime.Parse("1/1/2001"),
                Description = "Description1"
            };
            return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", awa1);
        }
        
        [HttpPost, ActionName("EditProject")]
        public IActionResult EditProjectForm([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", model);
        }
        [HttpPost, ActionName("EditCertificate")]
        public IActionResult EditCertificateForm([Bind("Name, StartDate, EndDate, Url")] model_Certificate model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", model);
        }
        [HttpPost, ActionName("EditActivity")]
        public IActionResult EditActivityForm([Bind("Name, Role, StartDate, EndDate, Description")] model_Activity model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", model);
        }
        [HttpPost, ActionName("EditAward")]
        public IActionResult EditAwardForm([Bind("Name, StartDate, EndDate, Description")] model_Award model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteContentNav(string navType, string id)
        {
            switch (navType)
            {
                case "Education":

                    break;
                case "Project":

                    break;
                case "Certificate":

                    break;
                case "Activity":

                    break;
                case "Award":

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
            TbCv cv;
            var result = fetcher.Update(out cv, model);
            if (result)
                return RedirectToAction(nameof(Index));
            return Conflict();
        }

        public IActionResult CreateProfile([Bind("LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email")] Profile model)
        {
            TbProfile profile;
            var result = fetcher.Create(out profile, model);
            if(result)
            return RedirectToAction(nameof(Index));
            return Conflict();
        }

        public IActionResult UpdateProfile([Bind(new[] { "LastName, FirstName, Gender, DoB, PhoneNumber, Address, SocialNetwork, DetailAddress, Email" })] Profile model)
        {
            TbProfile profile;
            var result = fetcher.Update(out profile, model);
            if (result)
                return RedirectToAction(nameof(Index));
            return Conflict();
        }
    }

}
