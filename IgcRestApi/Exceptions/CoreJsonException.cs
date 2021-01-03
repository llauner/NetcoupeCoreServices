using System;

namespace IgcRestApi.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CoreException" />
    public class CoreJsonException : CoreException
    {
        public CoreJsonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}