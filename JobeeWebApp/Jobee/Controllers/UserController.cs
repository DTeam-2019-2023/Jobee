using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobee.Controllers
{
    public interface IUserController
    {
        
        public IActionResult Index();
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

    }

    public class UserController : Controller, IUserController
    {
        public class UserCVModel
        {
            public Profile profile { get; set; }

            public string avtUrl { get; set; }

            public List<Education> Educations { get; set; } = default!;
            public List<Project> Projects { get; set; } = default!;
            public List<Certificate> Certificates { get; set; } = default!;
            public List<Activity> Activitys { get; set; } = default!;
            public List<Award> Awards { get; set; } = default!;
            public _ModelManagerment modelPopup { get; set; }
        }

        public UserCVModel _model { get; set; } = default!;
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {


            _model = new()
            {
                profile = new()
                {
                    FirstName = "Nguyen",
                    LastName = "Thanh Tai",
                    DoB = DateTime.Parse("1/1/2001"),
                    Gender = true,
                    PhoneNumber = "0123123123",
                    Address = "Cantho, Vietnam",
                    SocialNetwork = "fb.com/nguyenthanhtai"
                },
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
                        Gpa = 7.0,
                        Id = "edu2"
                    }
                },
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
                modelPopup = new()
            };
            return View(_model);
        }

        public IActionResult AddEducation([Bind("Name, Major, StartDate, EndDate, GPA, Description")] model_Education model)
        {
            return Ok(model);
        }

        public IActionResult AddProject([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            return Ok(model);
        }

        public IActionResult AddCertificate([Bind("Name, StartDate, EndDate, Url")] model_Certificate model)
        {
            return Ok(model);
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
            return Ok(model);
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
        [HttpPost("EditProject")]
        public IActionResult EditProjectForm([Bind("Name, TeamSize, Role, Technology, StartDate, EndDate, Description")] model_Project model)
        {
            if (ModelState.IsValid)
            {
                
            }
            return PartialView("~/Views/User/Popup/Edit/_editProject.cshtml", model);
        }
        [HttpPost("EditCertificate")]
        public IActionResult EditCertificateForm([Bind("Name, StartDate, EndDate, Url")] model_Certificate model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editCertificate.cshtml", model);
        }
        [HttpPost("EditActivity")]
        public IActionResult EditActivityForm([Bind("Name, Role, StartDate, EndDate, Description")] model_Activity model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editActivity.cshtml", model);
        }
        [HttpPost("EditAward")]
        public IActionResult EditAwardForm([Bind("Name, StartDate, EndDate, Description")] model_Award model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView("~/Views/User/Popup/Edit/_editAward.cshtml", model);
        }
    }

}
