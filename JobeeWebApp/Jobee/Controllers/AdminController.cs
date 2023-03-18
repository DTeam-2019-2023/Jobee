using Jobee_API.Entities;
using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AccountController;
using System.Text.Json;

namespace Jobee.Controllers
{
    public class AdminController : Controller
    {
        private Fetcher fetcher;
        public AdminController(IHttpContextAccessor context)
        {
            fetcher = new Fetcher(new Fetcher.ConfigFetcher()
            {
                context = context.HttpContext,
                root = "https://localhost:7063/api"
            });
        }

        public class VerifyContent
        {
            public string FullName { get; set; }
            public string Name { get; set; } //name cert
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }

        }
        public async Task<IActionResult> IndexAsync()
        {
            List<TbAccount> accounts;
            fetcher.GetAll(out accounts);
            ViewData["Users"] = accounts;
            List<VerifyContent> listVerifies = new List<VerifyContent>();

            Fetcher.Custom(async client =>
            {
                var res = await client.GetAsync($"https://localhost:7063/api/Admin/GetVerifies");
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(strData)) return;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    listVerifies = JsonSerializer.Deserialize<List<dynamic>>(strData, options).Select(i => new VerifyContent()
                    {
                        FullName = i.FullName,
                        Name = i.Name,
                        StartDate = i.StartDate,
                        EndDate = i.EndDate,
                        Description = i.Description,
                        Url = i.Url
                    }).ToList();
                }
            }).Wait();
                
            ViewData["Verifies"] = listVerifies;
            return View();
        }

    }
}
