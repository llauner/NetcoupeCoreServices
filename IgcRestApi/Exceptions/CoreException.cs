using System;

namespace IgcRestApi.Exceptions
{
    /// <summary>
    /// CoreException Exception
    /// </summary>
    public class CoreException : Exception
    {
        /// <summary>
        /// CoreException
        /// </summary>
        /// <param name="message"></param>
        public CoreException(string message) : base(message)
        {
        }

        /// <summary>
        /// CoreException
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}