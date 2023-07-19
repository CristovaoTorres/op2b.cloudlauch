using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Platform.Shared.Extensions
{
    public static class HttpExtensions
    {
        public async static Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(this HttpClient client, TRequest model, string url)
        {

            try
            {
                HttpResponseMessage response;

                response = await client.PutAsJsonAsync<TRequest>(url, model);

                return await ConvertTo<TResponse>(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }

        public async static Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest model, CancellationToken? cancellationToken = null)
        {

            try
            {
                HttpResponseMessage response;

                response = await client.PostAsJsonAsync<TRequest>(url, model, cancellationToken ?? CancellationToken.None);

                return await ConvertTo<TResponse>(response);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async static Task<TResponse> GetFromJsonAsyncExternal<TResponse>(this HttpClient client, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"The request failed with status code {response.StatusCode}");
                }

                string responseContent = await response.Content.ReadAsStringAsync();

                // Use uma biblioteca de serialização JSON para desserializar a string em um objeto do tipo TResponse
                TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent);

                return responseObject;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async static Task<T> ConvertTo<T>(this HttpResponseMessage response)
        {

            try
            {

                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString != null)
                {
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
