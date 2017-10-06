using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace SandTigerShark.Functions.Tests
{
    public static class HttpRequest
    {
        public static HttpRequestMessage Create<T>(T objectToPost)
        {
            var json = JsonConvert.SerializeObject(objectToPost);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            return request;
        }
    }
}
