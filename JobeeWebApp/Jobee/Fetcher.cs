using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static Jobee.Controllers.AccountController;

namespace Jobee
{
    public class Fetcher
    {
        private static Fetcher instance = default!;
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

        public static async Task<(string, string)> LoginAsync(SigninModel model)
        {
         var client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            
            var respone = await client.PostAsJsonAsync(requestUri: "https://localhost:7063/api/Users/login", model);
            string strData = await respone.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(strData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return (result.token.ToString(), result.type.ToString());
        }
        public static async Task<bool> SignupAsync(SignupModel model)
        {
            var client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var respone = await client.PostAsJsonAsync(requestUri: "https://localhost:7063/api/Users/signup", model);
            return respone.IsSuccessStatusCode;
        }
        public async Task<bool> LogoutAsync()
        {
            var result = await client.PostAsync(requestUri: $"{ApiUrl["root"]}/Users/logout", null);
            return true;
        }
        public void GetAll<T>(out List<T> objs)
        {
            objs = GetAllAsync<T>().Result;
        }
        public void GetById<T>(out T obj, string id)
        {
            obj = GetByIdAsync<T>(typeof(T).Name, id).Result;
        }

        public void GetSingleAuto<T>(out T obj)
        {
            obj = GetSingleAutoAsync<T>().Result;
        }

        private async Task<T> GetSingleAutoAsync<T>()
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(GetSingleAuto)}";
            var res = await client.GetAsync(requestUri: builder.Uri);
            string strData = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (strData == "null")
            {
                return default!;
            }
            var values = JsonSerializer.Deserialize<T>(strData, options);
            return values!;
        }

        private async Task<T> GetByIdAsync<T>(string objName, string id)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{objName}/{"1"}";
            var res = await client.GetAsync(requestUri: builder.Uri);
            string strData = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var values = JsonSerializer.Deserialize<T>(strData, options);
            return values!;
        }

        public bool Create<T,U>(out T result, U Data)
        {
            result = CreateAsync<T,U>( Data).Result;
            return true;
        }
        
        public async Task<T> CreateAsync<T,U>(U Data)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(Create)}";

            var res = await client.PostAsJsonAsync(requestUri: builder.Uri, Data);
            string strData = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var values = JsonSerializer.Deserialize<T>(strData, options);
            return values!;
        }

        public bool Update<T, U>(out T result, U Data)
        {
            result = UpdateAsync<T, U>(Data).Result;
            return true;
        }

        public async Task<T> UpdateAsync<T, U>(U Data)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Path += $"/{typeof(T).Name}/{nameof(Update)}";

            var res = await client.PutAsJsonAsync(requestUri: builder.Uri, Data);
            string strData = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var values = JsonSerializer.Deserialize<T>(strData, options);
            return values!;
        }

        private async Task<List<T>> GetAllAsync<T>()
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            HttpResponseMessage r = await client.GetAsync(builder.Uri);
            string strData = await r.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var values = JsonSerializer.Deserialize<List<T>>(strData, options);
            return values!;
        }

        public bool Remove(string key)
        {
            _ = RemoveAsync(key);
            return true;
        }

        public async Task RemoveAsync(string key)
        {
            UriBuilder builder = new UriBuilder(ApiUrl["root"]);
            builder.Query = $"key={key}";
            var res = await client.DeleteAsync(requestUri: builder.Uri);
        }
    }
}
