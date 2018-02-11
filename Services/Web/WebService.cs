using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Services.Web
{
    public class WebService : IWebService
    {
        private static readonly HttpClient client = new HttpClient();

        public T GetDeserialized<T>(string endpoint) where T : class
        {
            var responseString = client.GetStringAsync(endpoint).Result;
            var obj = JsonConvert.DeserializeObject<T>(responseString);

            return obj;
        }

        public string SendSerialized<T>(string endpoint, T obj) where T : class
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = client.PostAsync(new Uri(endpoint), content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }
    }
}
