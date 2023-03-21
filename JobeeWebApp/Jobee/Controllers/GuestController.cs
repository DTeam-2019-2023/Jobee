using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AdminController;
using System.Text.Json;
using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.Extensions.FileSystemGlobbing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;

namespace Jobee.Controllers
{
    public interface IGuestController
    {
        public IActionResult ViewEducation(string id);
        public IActionResult ViewProject(string id);
        public IActionResult ViewCertificate(string id);
        public IActionResult ViewActivity(string id);
        public IActionResult ViewAward(string id);
    }
    public class GuestController : Controller
    {
        private Fetcher fetcher;
        public GuestController(IHttpContextAccessor context)
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
        public UserCVModel _model { get; set; } = new();


        [Route("/Guest/{username}")]
        public async Task<IActionResult> IndexAsync(string username)
        {
            bool isError = false;
            TbProfile profile = new TbProfile();
            TbCv cv = new TbCv();
            List<Education> edu = new List<Education>();
            List<Project> project = new List<Project>();
            List<Certificate> cert = new List<Certificate>();
            List<Activity> activity = new List<Activity>();
            List<Award> award = new List<Award>();
            await Fetcher.Custom(async client =>
            {
                var res = client.GetAsync($"https://localhost:7063/api/Guest/UserProfile?username={username}").Result;
                if(res.StatusCode == System.Net.HttpStatusCode.NotFound) 
                {
                    isError = true;
                    HttpContext.Response.StatusCode = 404;
                }
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(strData)) return;
                    //dynamic temp = JObject.Parse(strData);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true,
                        IncludeFields = true
                    };
                  var result = JsonSerializer.Deserialize<(TbProfile, TbCv, List<Education>, List<Project>, List<Certificate>, List<Activity>, List<Award>)>(strData, options);
                    profile = result.Item1;
                    cv = result.Item2;  
                    edu = result.Item3;
                    project = result.Item4;
                    cert = result.Item5;
                    activity = result.Item6;
                    award = result.Item7;
                }
            });

            if(isError)
            {
                return NotFound();
            }

            if (profile != null)
            {
                _model.profile = new()
                {
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DoB = profile.DoB,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumber,
                    Address = profile.Address,
                    SocialNetwork = profile.SocialNetwork,
                    Email = profile.Email,
                    DetailAddress = profile.DetailAddress,
                };
            }

            if (cv != null)
            {
                _model.general = new()
                {
                    ApplyPosition = cv.ApplyPosition,
                    CurrentJob = cv.CurrentJob,
                    DesirySalary = cv.DesirySalary,
                    Degree = cv.Degree,
                    WorkExperience = cv.WorkExperience,
                    DesiredWorkLocation = cv.DesiredWorkLocation,
                    WorkingForm = cv.WorkingForm,
                    CarrerObjiect = cv.CarrerObject,
                    SoftSkill = cv.SoftSkill,
                    Avatar = "/images/Members/1w2ta6TdDwqezXFEeQYVJw.jpg"
                };
            }

            if(edu != null)
            {
                _model.Educations = edu;
            }

            if(project != null)
            {
                _model.Projects = project;
            }
            
            if(cert != null)
            {
                _model.Certificates = cert;
            }

            if(activity != null)
            {
                _model.Activitys = activity;
            }

            if(award != null)
            {
                _model.Awards = award;
            }
            return View(_model);
        }

        //=================//
        [HttpPost]
        [Route("/Guest/ViewEducation")]
        [IgnoreAntiforgeryToken]
        public ActionResult ViewEducation(string id)
        {
            Education data;
            //await Fetcher.Custom(async client =>
            //{
            //    var res = client.GetAsync($"https://localhost:7063/api/GetById/{id}").Result;

            //    if (res.StatusCode == System.Net.HttpStatusCode.OK)
            //    {
            //        string strData = await res.Content.ReadAsStringAsync();
            //        if (string.IsNullOrEmpty(strData)) return;
            //        //dynamic temp = JObject.Parse(strData);
            //        var options = new JsonSerializerOptions
            //        {
            //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //            PropertyNameCaseInsensitive = true,
            //            IncludeFields = true
            //        };

            //    }
            //});
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

        public IActionResult ViewProject(string id)
        {
            Project data;
            throw new NotImplementedException();
        }

        public IActionResult ViewCertificate(string id)
        {
            Certificate data;
            throw new NotImplementedException();
        }

        public IActionResult ViewActivity(string id)
        {
            Activity data;
            throw new NotImplementedException();
        }

        public IActionResult ViewAward(string id)
        {
            Award data;
            throw new NotImplementedException();
        }
        
    }
}