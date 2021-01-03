using IgcRestApi.Common.Helper;
using System;
using System.Net;

namespace IgcRestApi.Models
{
    public class CoreApiExceptionModel
    {
        private string _stackTrace;
        private readonly bool _hideStackTrace = true;

        public DateTime DateTime { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public string StackTrace
        {
            get => _stackTrace;
            set => _stackTrace = (_hideStackTrace) ? null : value;
        }


        public override string ToString()
        {
            return JsonHelper.Serialize(this);
        }
    }
}
