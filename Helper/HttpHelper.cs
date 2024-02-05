using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EmployeeAPI.Helper
{
    public class BaseHttpClient
    {
        HttpClient _httpClient;
        private readonly string _secureCode = string.Empty;
        public BaseHttpClient(string BaseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public BaseHttpClient(string BaseUrl, string accessToken, string authType)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(authType))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _httpClient.DefaultRequestHeaders.Add("AuthType", authType);
            }
        }
        public BaseHttpClient(string BaseUrl, string code, string accessToken, string authType)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(authType))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _httpClient.DefaultRequestHeaders.Add("AuthType", authType);
            }
            _secureCode = code;
        }
        public async Task<R> Post<T, R>(string Method, T Data)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}/code={_secureCode}", CreateHttpContent(Data));
            if (response.IsSuccessStatusCode)
            {
                // Get the URI of the created resource.
                Uri returnUrl = response.Headers.Location;
                return response.Content.ReadFromJsonAsync<R>().Result;
            }
            return default(R);
        }

        public async Task<R> Get<R>(string Method)
        {
            string _Method = string.Empty;

            if (Method.Contains('?'))
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}&code={_secureCode}";
            else
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}?code={_secureCode}";

            //GET Method
            HttpResponseMessage response = await _httpClient.GetAsync(_Method);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<R>().Result;
            }
            else
            {
                return default(R);
            }
        }
        public async Task<int> PostStatus<T, R>(string Method, T Data)
        {
            string _Method = string.Empty;

            if (Method.Contains('?'))
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}&code={_secureCode}";
            else
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}?code={_secureCode}";
            HttpResponseMessage response = await _httpClient.PostAsync(string.IsNullOrEmpty(_secureCode) ? Method : $"{_Method}", CreateHttpContent(Data));
            if (response.IsSuccessStatusCode)
            {
                // Get the URI of the created resource.
                Uri returnUrl = response.Headers.Location;
                return response.Content.ReadFromJsonAsync<int>().Result;
            }
            return 0;
        }

        public async Task<int> GetStatus<R>(string Method)
        {
            string _Method = string.Empty;

            if (Method.Contains('?'))
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}&code={_secureCode}";
            else
                _Method = string.IsNullOrEmpty(_secureCode) ? Method : $"{Method}?code={_secureCode}";

            //GET Method
            HttpResponseMessage response = await _httpClient.GetAsync(_Method);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<int>().Result;
            }
            else
            {
                return response.Content.ReadFromJsonAsync<int>().Result;
            }
        }
        void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Newtonsoft.Json.Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        public HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return httpContent;
        }
    }
}