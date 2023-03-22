using Jobee_API.Entities;
using Microsoft.AspNetCore.Mvc;
using static Jobee.Controllers.AccountController;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace Jobee.Controllers
{
    [Authorize(Roles = "ad")]
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
        List<string> DesiredWorkLocations = new List<string>() { "An Giang", "Bà Rịa-Vũng Tàu", "Bạc Liêu", "Bắc Kạn", "Bắc Giang", "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An", "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa", "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái", "Phú Quốc", "Đà Nẵng", "Hải Phòng", "Hà Nội", "Thành phố Hồ Chí Minh", "Cần Thơ" };

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
        public async Task<IActionResult> IndexAsync(string SearchText = default!)
        {
            List<TbAccount> accounts;
            fetcher.GetAll(out accounts);

            if (SearchText != "" && SearchText != null)
            {
                accounts = accounts.Where(p => p.Username.Contains(SearchText)).ToList();
            }

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
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, "#");
            ViewData["Verified"] = listVerifies;
            ViewData["Users"] = accounts;

            return View();
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
                var res = await client.PutAsJsonAsync("https://localhost:7063/api/Admin/VerifyCertificate", id);
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
        [HttpPost, ActionName("CreateAdmin")]
        public IActionResult PostCreateAdminForm([Bind("Username, Password, rePassword, Firstname, Lastname, dob, Gender, Address, PhoneNumber, email, DetailAddress")] SignupAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var result = Fetcher.SignupAdminAsync(model, "https://localhost:7063/api/Admin/signup").Result;
                if (result == (int)HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result == (int)HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(nameof(model.Username), "Admin account have been exist");
                }
            }
            ModelState.AddModelError("CreateAdmin", "not valid");
            ViewData["DesiredWorkLocations"] = getListItem("Desired Work Location", DesiredWorkLocations, "#");
            return View(nameof(Index));
        }
    }
}
