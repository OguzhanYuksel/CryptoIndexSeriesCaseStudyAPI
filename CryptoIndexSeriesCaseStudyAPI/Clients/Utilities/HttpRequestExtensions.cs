using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Utilities
{
    public static class HttpRequestExtensions
    {
        public static ByteArrayContent ContentAsByte(object value, string mediaType = "application/json")
        {
            string message = JsonConvert.SerializeObject(value);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return content;
        }

        public static ByteArrayContent ContentAsByteJson(object value, string mediaType = "application/json")
        {
            string message = JsonConvert.SerializeObject(value);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return content;
        }
    }
}
