using Jobee_API.Entities;
using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AccountController;
using System.Text.Json;
using Newtonsoft.Json.Linq;

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
            List<VerifyContent> listVerifies = new List<VerifyContent>();

            await Fetcher.Custom(async client =>
            {
                var res = client.GetAsync($"https://localhost:7063/api/Admin/GetVerifies").Result;
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(strData)) return;
                    dynamic temp = JObject.Parse(strData);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    listVerifies = JsonSerializer.Deserialize<List<VerifyContent>>(temp, options);
                    //listVerifies = result.Select(i => new VerifyContent()
                    //{
                    //    FullName = i.fullName,
                    //    Name = i.name,
                    //    StartDate = i.startDate,
                    //    EndDate = i.endDate,
                    //    Description = i.description,
                    //    Url = i.url
                    //}).ToList();
                }
            });
            ViewData["Verifies"] = listVerifies;
            ViewData["Users"] = accounts;

            return View();
        }

    }
}
