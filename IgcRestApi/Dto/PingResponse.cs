using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgcRestApi.Dto
{
    /// <summary>
    /// Ping Response
    /// </summary>
    public class PingResponse
    {
        public DateTime DateTime { get; set; }

        public string Message { get; set; }

        public PingResponse(string message)
        {
            DateTime = DateTime.UtcNow;
            Message = message;
        }
    }
}
