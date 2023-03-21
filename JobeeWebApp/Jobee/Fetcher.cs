using Jobee_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using static Jobee.Controllers.AccountController;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobee
{
    public class Fetcher
    {
        private readonly HttpClient client = null!;
        public Dictionary<string, string> ApiUrl;
        public class ConfigFetcher
        {
            public string root { get; set; } = default!;
            public HttpContext context { get; set; } = default!;

        }

        public Fetcher(ConfigFetcher config)
        {
            client = new HttpClient();
            var token = config.context.Request.Cookies["jwt"];
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(contentType);
            //Url
            ApiUrl = new();
            ApiUrl.Add("root", config.root);
        }
        
       

        public static async Task<(string, string)> LoginAsync(User model, string loginUri)
        {
            var client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var respone = await client.PostAsJsonAsync(requestUri: loginUri, model);
            if (respone.IsSuccessStatusCode)
            {
                string strData = await respone.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                dynamic result = Newtonsoft.Json.Linq.JObject.Parse(strData);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return (result.token.ToString(), result.type.ToString());
            }
            return default!;
        }

        public static async Task<bool> SignupAsync(SignupModel model, string signupUri)
        {
            var client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var respone = await client.PostAsJsonAsync(requestUri: signupUri, model);
            return respone.IsSuccessStatusCode;
        }
        public static async Task<bool> SignupAdminAsync(SignupAdminModel model, string signupUri)
        {
            var client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //đang lỗi ở đây
            var respone = await client.PostAsJsonAsync(requestUri: signupUri, model);
            return respone.IsSuccessStatusCode;
        }
        public async Task<bool> LogoutAsync()
        {
            var result = await client.PostAsync(requestUri: $"{ApiUrl["root"]}/Users/logout", null);
            return true;
        }

        public static Task Custom(Action<HttpClient> Client)
        {
            var _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            Client(_client);

            return Task.CompletedTask;
        }
        //[HttpGet]
        //[Route("GetAll")]
        public void GetAll<T>(out List<T> objs) where T : class
        {
            objs = GetAllAsync<T>().Result;
        }
        //[HttpGet]
        //[Route("GetById/{id}")]
        public void GetById<T>(out T obj, string id) where T : class
        {
            obj = GetByIdAsync<T>(id).Result;
        }
        //[HttpGet]
        //[Route("GetSingleAuto")]
        public void GetSingleAuto<T>(out T obj) where T : class
        {
            obj = GetSingleAutoAsync<T>().Result;
        }
        //[HttpPost]
        //[Route("Create")]
        public bool Create<T, U>(out T result, U Data) where T : class where U : notnull
        {
            result = CreateAsync<T, U>(Data).Result;
            return true;
        }
        //[HttpPut]
        //[Route("Update")]
        public bool Update<T, U>(out T result, U Data) where T : class where U : notnull
        {
            result = UpdateAsync<T, U>(Data).Result;
            return true;
        }
        //[HttpPut]
        //[Route("UpdateById/{id}")]
        public bool UpdateById<T, U>(out T result, U Data, string id) where T : class where U : notnull
        {
            result = UpdateByIdAsync<T, U>(Data, id).Result;
            return true;
        }
        //[HttpDelete]
        //[Route("Remove/{id}")]
        public bool Remove<T>(string id)
        {
            _ = RemoveAsync<T>(id);
            return true;
        }

        #region private 
        private async Task<T> GetByIdAsync<T>(string id)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(GetById)}/{id}";
            var res = await client.GetAsync(requestUri: builder.Uri);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<T>(strData, options);
                return values!;
            }
            return default!;
        }
        private async Task<T> GetSingleAutoAsync<T>()
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(GetSingleAuto)}";
            var res = await client.GetAsync(requestUri: builder.Uri);
            if (res.IsSuccessStatusCode)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<T>(strData, options);
                return values!;
            }
            return default!;

        }
        private async Task<T> CreateAsync<T, U>(U Data)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(Create)}";

            var res = await client.PostAsJsonAsync(requestUri: builder.Uri, Data);
            if (res.IsSuccessStatusCode)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<T>(strData, options);
                return values!;
            }
            return default!;
        }
        private async Task<T> UpdateAsync<T, U>(U Data)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(Update)}";

            var res = await client.PutAsJsonAsync(requestUri: builder.Uri, Data);
            if (res.IsSuccessStatusCode)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<T>(strData, options);
                return values!;
            }
            return default!;
        }
        private async Task<T> UpdateByIdAsync<T, U>(U Data, string id)
            where T : class
            where U : notnull
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(UpdateById)}/{id}";

            var res = await client.PutAsJsonAsync(requestUri: builder.Uri, Data);
            if (res.IsSuccessStatusCode)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<T>(strData, options);
                return values!;
            }
            return default!;
        }
        private async Task<List<T>> GetAllAsync<T>()
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(GetAll)}";

            HttpResponseMessage res = await client.GetAsync(builder.Uri);
            if (res.IsSuccessStatusCode)
            {
                string strData = await res.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(strData)) return default!;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var values = JsonSerializer.Deserialize<List<T>>(strData, options);
                return values!;
            }
            return default!;
        }
        private async Task RemoveAsync<T>(string id)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(Remove)}/{id}";

            var res = await client.DeleteAsync(requestUri: builder.Uri);
        }
        #endregion
    }
}
