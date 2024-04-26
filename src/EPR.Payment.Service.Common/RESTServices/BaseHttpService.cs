using EPR.Payment.Service.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.RESTServices
{
    public abstract class BaseHttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public BaseHttpService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            string baseUrl,
            string endPointName)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            // do basic checks on parameters
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? throw new ArgumentNullException(nameof(baseUrl)) : baseUrl;

            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (string.IsNullOrWhiteSpace(endPointName))
                throw new ArgumentNullException(nameof(endPointName));

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            if (_baseUrl.EndsWith("/"))
                _baseUrl = _baseUrl.TrimEnd('/');

            _baseUrl = $"{_baseUrl}/{endPointName}";
        }

        /// <summary>
        /// Performs an Http GET returning the specified object
        /// </summary>
        protected async Task<T> Get<T>(string url, bool includeTrailingSlash = true)
        {
            url = includeTrailingSlash ? $"{_baseUrl}/{url}/" : $"{_baseUrl}/{url}";

            return await Send<T>(CreateMessage(url, null, HttpMethod.Get));
        }

        /// <summary>
        /// Performs an Http POST returning the speicified object
        /// </summary>
        protected async Task<T> Post<T>(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Post));
        }

        /// <summary>
        /// Performs an Http POST returning the speicified object
        /// </summary>
        protected async Task<T> Post<T>(object payload = null)
        {
            var url = $"{_baseUrl}";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Post));
        }

        /// <summary>
        /// Performs an Http POST without returning any data
        /// </summary>
        protected async Task Post(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            await Send(CreateMessage(url, payload, HttpMethod.Post));
        }

        /// <summary>
        /// Performs an Http PUT returning the speicified object
        /// </summary>
        protected async Task<T> Put<T>(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Put));
        }

        /// <summary>
        /// Performs an Http PUT without returning any data
        /// </summary>
        protected async Task Put(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            await Send(CreateMessage(url, payload, HttpMethod.Put));
        }

        /// <summary>
        /// Performs an Http DELETE returning the speicified object
        /// </summary>
        protected async Task<T> Delete<T>(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Delete));
        }

        /// <summary>
        /// Performs an Http DELETE without returning any data
        /// </summary>
        protected async Task Delete(string url, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            await Send(CreateMessage(url, payload, HttpMethod.Delete));
        }

        private HttpRequestMessage CreateMessage(
            string url,
            object payload,
            HttpMethod httpMethod)
        {
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = httpMethod
            };

            if (payload != null)
            {
                msg.Content = JsonContent.Create(payload);
            }

            return msg;
        }

        private async Task<T> Send<T>(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(responseStream);
                var content = await streamReader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(content))
                    return default!;

                return ReturnValue<T>(content);
            }
            else
            {
                // get any message from the response
                var responseStream = await response.Content.ReadAsStreamAsync();
                var content = default(string);

                if (responseStream.Length > 0)
                {
                    using var streamReader = new StreamReader(responseStream);
                    content = await streamReader.ReadToEndAsync();
                }

                // set the response status code and throw the exception for the middleware to handle
                throw new ResponseCodeException(response.StatusCode, content);
            }
        }

        private async Task Send(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)response.StatusCode;
                // for now we don't know how we're going to handle errors specifically,
                // so we'll just throw an error with the error code
                throw new Exception($"Error occurred calling API with error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
            }
        }

        private T ReturnValue<T>(string value)
        {
            if (IsValidJson(value))
                return JsonConvert.DeserializeObject<T>(value)!;
            else
                return (T)Convert.ChangeType(value, typeof(T));
        }

        private bool IsValidJson(string stringValue)
        {
            try
            {
                var val = JToken.Parse(stringValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string BuildUrlWithQueryString(object dto)
        {
            var properties = dto.GetType().GetProperties()
                .Where(p => p.GetValue(dto, null) != null)
                .Select(p => p.Name + "=" + Uri.EscapeDataString(p.GetValue(dto, null).ToString()));
            return "?" + string.Join("&", properties);
        }
    }
}
