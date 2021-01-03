using System;
using System.Net;

namespace IgcRestApi.Exceptions
{
    /// <summary>
    /// CoreApiException
    /// Thrown when an exception occurs based on an http status code
    /// </summary>
    public class CoreApiException : Exception
    {
        /// <summary>
        /// Get DateTime when error occurs
        /// </summary>
        public DateTime DateTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Get status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// StackTrace
        /// Add option to hide the StackTrace
        /// </summary>
        public override string StackTrace => _hideStackTrace ? null : base.StackTrace;

        private readonly bool _hideStackTrace;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public CoreApiException(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="message"></param>
        public CoreApiException(HttpStatusCode httpStatusCode, string message)
            : base(message)
        {
            StatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="message"></param>
        /// <param name="hideStackTrace"></param>
        public CoreApiException(HttpStatusCode httpStatusCode, string message, bool hideStackTrace)
            : base(message)
        {
            StatusCode = httpStatusCode;
            _hideStackTrace = hideStackTrace;
        }
    }
}
