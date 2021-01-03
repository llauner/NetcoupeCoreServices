using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;

namespace IgcRestApi.Models
{
    public class ApiResponseModel
    {
        public object Result { get; }

        public HttpStatusCode StatusCode { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpStatusCode HttpStatusCode => StatusCode;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponseModel(HttpStatusCode statusCode, object result = null, string message = null)
        {
            StatusCode = statusCode;
            Result = result;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return "[IgcRestApi] Resource not found";
                case HttpStatusCode.InternalServerError:
                    return "[IgcRestApi] An unhandled error occurred";
                default:
                    return null;
            }
        }


    }
}
