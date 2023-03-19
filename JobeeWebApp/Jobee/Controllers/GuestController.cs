using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AdminController;
using System.Text.Json;
using Jobee_API.Entities;
using Jobee_API.Models;

namespace Jobee.Controllers
{
    public class GuestController : Controller
    {
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
    }
}