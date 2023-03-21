using Jobee_API.Entities;
using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AccountController;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Net.Http.Headers;

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
            public string Id { get; set; }
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
                    //dynamic temp = JObject.Parse(strData);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<List<VerifyContent>>(strData, options);
                    if (result != null)
                    {
                        listVerifies = result;
                    }
                }
            });
            ViewData["Verified"] = listVerifies;
            ViewData["Users"] = accounts;

            return View();
        }

        public async Task<IActionResult> VerifyCertificateAsync(string id)
        {
            Certificate obj;
            fetcher.GetById(out obj, id);
            if (obj != null)
            {
                obj.IsVertify = true;
            }
            await Fetcher.Custom(async client =>
            {
                var token = HttpContext.Request.Cookies["jwt"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //Url
               var res =  await client.PutAsJsonAsync("https://localhost:7063/api/Admin/VerifyCertificate", id);
                if (res.IsSuccessStatusCode)
                {
                    string strData = await res.Content.ReadAsStringAsync();
                }
            });
            //Certificate result;
            //fetcher.Update(out result, a);

            //fetcher.Update(out a, new Certificate { Id = id, IsVertify = true });
            //Kawait Fetcher.Custom(async client =>
            //{
            //    var res = client.GetAsync($"https://localhost:7063/api/Admin/VerifyCertificate/{id}").Result;
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
            //        var result = JsonSerializer.Deserialize<Certificate>(strData, options);
            //    }
            //});
            return Ok(new { status = "success", id = id });
        }
    }
}
