using BaseClient.Exceptions;
using Hvt.Utilities;
using System.Net.Http.Headers;
using System.Text;

namespace BaseClient
{
    public class BaseClient
    {
        protected readonly HttpClient Client;
        protected readonly string? Token = null;
        public BaseClient(HttpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            if (Client.DefaultRequestHeaders.TryGetValues("X-Token", out var values))
            {
                Token = values.FirstOrDefault();
                Client.DefaultRequestHeaders.Remove("X-Token");
            }
        }

        #region Internal Methods
        /// <summary>
        /// Execute an HTTP request to the API and return the response as a deserialized object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<T?> Execute<T>(string endpoint, HttpMethod method, object? payload = null, CancellationToken cancellationToken = default) where T : class
        {
            string? json = await SendRequest(endpoint, method, payload, cancellationToken);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return Serializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Execute an HTTP request to the API and return the response as string
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BaseClientException"></exception>
        protected async Task<string?> SendRequest(string endpoint, HttpMethod method, object? payload = null, CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, endpoint);
            //Add Headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: Token);
            }
            //Add body if needed
            if (payload != null)
            {
                string json = Serializer.Serialize(payload);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            //Execute request
            HttpResponseMessage response = await Client.SendAsync(request, cancellationToken);
            //Read response
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            //Return response
            if (response.IsSuccessStatusCode)
            {
                return (string.IsNullOrEmpty(content) || content.Equals("{}")) ? "true" : content;
            }
            else
            {
                throw new BaseClientException(content);
            }
        }
        #endregion
    }
}
